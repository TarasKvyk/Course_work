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

        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) // dependency injection
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
                    bookList = bookList.OrderBy(b => b.Author.Name).ToList();
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
            
            if(bookToDelete == null)
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
                Text = u.Name + " - " + u.Specialization,
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
                // create 
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
                if(file == null && BookVM.Book.Id == 0)
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
    }
}
