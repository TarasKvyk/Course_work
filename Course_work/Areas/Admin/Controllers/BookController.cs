using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Globalization;


namespace Course_work.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int orderOptionId = 0)
        {
            List<Book> bookList = _unitOfWork.Book.GetAll(includeProperties: "Category,Author").ToList();

            switch (orderOptionId)
            {
                case 1:
                    bookList = bookList.OrderBy(b => b.Title).ToList();
                    break;
                case 2:
                    bookList = bookList.OrderBy(b => b.Author.Surname).ToList();
                    break;
                case 3:
                    bookList = bookList.OrderBy(b => b.Category.Name).ToList();
                    break;
                case 4:
                    bookList = bookList.OrderBy(b => b.Year).ToList();
                    break;
                case 5:
                    bookList = bookList.OrderBy(b => b.Language).ToList();
                    break;
                case 6:
                    bookList = bookList.OrderBy(b => b.Price).ToList();
                    break;
                default:
                    break;
            }

            return View(bookList);
        }

        public IActionResult Delete(int? bookId)
        {
            if (bookId == 0 || bookId == null)
                return NotFound();

            Book bookToDelete = _unitOfWork.Book.Get(b => b.Id == bookId);

            if (bookToDelete == null)
                return NotFound();

            _unitOfWork.Book.Remove(bookToDelete);
            _unitOfWork.Save();

            TempData["success"] = $"Book \"{bookToDelete.Title}\" has been deleted successfully";
            return RedirectToAction("Index", "Book");
        }

        public IActionResult Upsert(int? bookId)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().OrderBy(c => c.Name).Select(u => new SelectListItem
            {
                Text = u.Name.Replace("Category", "") + " - " + u.Specialization,
                Value = u.Id.ToString()
            });

            var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures).OrderBy(c => c.EnglishName);

            IEnumerable<SelectListItem> languageList = cultures.Select(c => new SelectListItem
            {
                Text = c.EnglishName,
                Value = c.TwoLetterISOLanguageName
            });

            IEnumerable<SelectListItem> authorList = _unitOfWork.Auhtor.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name + " " + u.Surname,
                Value = u.Id.ToString()
            });

            BookVM bookVM = new BookVM()
            {
                Book = new Book(),
                AuthorList = authorList,
                CategoryList = categoryList,
                LanguageList = languageList
            };

            if (bookId == 0 || bookId == null)
            {
                return View(bookVM);
            }
            else
            {
                bookVM.Book = _unitOfWork.Book.Get(b => b.Id == bookId, includeProperties: "Category,Author");
                return View(bookVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(BookVM BookVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file == null && BookVM.Book.Id == 0)
                {
                    BookVM.CategoryList = _unitOfWork.Category.GetAll().OrderBy(c => c.Name).Select(u => new SelectListItem
                    {
                        Text = u.Name.Replace("Category", "") + " - " + u.Specialization,
                        Value = u.Id.ToString()
                    });

                    var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

                    BookVM.LanguageList = cultures.Select(c => new SelectListItem
                    {
                        Text = c.EnglishName,
                        Value = c.TwoLetterISOLanguageName
                    });

                    BookVM.AuthorList = _unitOfWork.Auhtor.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name + " " + u.Surname,
                        Value = u.Id.ToString()
                    });

                    TempData["error"] = $"Upload \"{BookVM.Book.Title}\" photo";

                    return View(BookVM);
                }

                if (BookVM.Book.Id == 0)
                {
                    _unitOfWork.Book.Add(BookVM.Book);
                    _unitOfWork.Save();
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"images\book\book-" + BookVM.Book.Id;
                    string finalPath = Path.Combine(wwwRootPath, productPath);

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    BookVM.Book.ImageUrl = @"\" + productPath + @"\" + fileName;
                }

                _unitOfWork.Book.Update(BookVM.Book);
                _unitOfWork.Save();


                TempData["success"] = $"Product \"{BookVM.Book.Title}\" created/updated successfully";
                return RedirectToAction("Index", "Book");
            }
            else
            {
                BookVM.CategoryList = _unitOfWork.Category.GetAll().OrderBy(c => c.Name).Select(u => new SelectListItem
                {
                    Text = u.Name + " - " + u.Specialization,
                    Value = u.Id.ToString()
                });

                var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

                BookVM.LanguageList = cultures.Select(c => new SelectListItem
                {
                    Text = c.EnglishName,
                    Value = c.ThreeLetterISOLanguageName
                });

                BookVM.AuthorList = _unitOfWork.Auhtor.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name + " " + u.Surname,
                    Value = u.Id.ToString()
                });

                TempData["error"] = $"Invalid \"{BookVM.Book.Title}\" data";

                return View(BookVM);
            }
        }

        public IActionResult DeleteImage(int? bookId)
        {
            if (bookId != 0 && bookId != null)
            {
                Book bookFromDb = _unitOfWork.Book.Get(b => b.Id == bookId);
                string? imageToDeleteUrl = Path.Combine(_webHostEnvironment.WebRootPath, bookFromDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(imageToDeleteUrl))
                    System.IO.File.Delete(imageToDeleteUrl);

                bookFromDb.ImageUrl = "";
                _unitOfWork.Book.Update(bookFromDb);
                _unitOfWork.Save();

                TempData["success"] = $"Image for \"{bookFromDb.Title}\" has been deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { bookId = bookId });
        }

        [HttpPost]
        public IActionResult WriteIntoFile(IFormFile file)
        {
            if (file == null)
            {
                TempData["error"] = $"Incorect file!";
                return RedirectToAction("Index");
            }
            
            string filePath = @"uploads\" + file.FileName;

            //if (!Directory.Exists(filePath))
            //    Directory.CreateDirectory(filePath);

            List<Book> books = _unitOfWork.Book.GetAll(includeProperties: "Author,Category").ToList();

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string headerLine = $"Id|Title|Page Count|Available Count|Price|CategoryId|Year|AuthorId|Language";
                writer.WriteLine(headerLine);

                foreach (var book in books)
                {
                    string line = $"{book.Id}|{book.Title}|{book.PageCount}|{book.AvailableCount}|{book.Price}|{book.CategoryId}|{book.Year}|{book.AuthorId}|{book.Language}";
                    writer.WriteLine(line);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ReadFromFile(IFormFile file)
        {
            if (file == null)
            {
                TempData["error"] = $"Incorect file!";
                return RedirectToAction("Index");
            }

            string filePath = @"uploads\" + file.FileName;
            string[] lines = System.IO.File.ReadAllLines(filePath);

            try
            {
                lines = System.IO.File.ReadAllLines(filePath);
            }
            catch (Exception e)
            {
                TempData["error"] = $"Can not find file!";
                return RedirectToAction("Index");
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('|');

                try
                {
                    int bookId = int.Parse(values[0].Trim());
                    
                    if (_unitOfWork.Book.Get(b => b.Id == bookId) == null)
                    {
                        Book book = new Book
                        {
                            Title = values[1].Trim(),
                            PageCount = int.Parse(values[2].Trim()),
                            AvailableCount = int.Parse(values[3].Trim()),
                            Price = double.Parse(values[4].Trim()),
                            CategoryId = int.Parse(values[5].Trim()),
                            Year = int.Parse(values[6].Trim()),
                            AuthorId = int.Parse(values[7].Trim()),
                            Language = values[8].Trim()
                        };

                        _unitOfWork.Book.Add(book);
                    }
                }
                catch(Exception e)
                {
                    TempData["error"] = $"Error while reading file! Check file data int row " + i + 1;
                    return RedirectToAction("Index");
                }

            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}