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
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthorController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) // dependency injection
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Author> authorList = _unitOfWork.Auhtor.GetAll().ToList();

            return View(authorList);
        }

        public IActionResult Delete(int? authorId)
        {
            if (authorId == 0 || authorId == null)
                return NotFound();

            var BooksWithThatAuthor = _unitOfWork.Book.GetAll(b => b.AuthorId == authorId).ToList();

            if (_unitOfWork.Auhtor.Get(a => a.Name == "Unknown") == null)
            {
                Author unknownAuthor = new Author()
                {
                    Id = 0,
                    Name = "Unknown",
                    Surname = "Unknown",
                    Country = "Unknown",
                    BirthDate = DateTime.MinValue
                };

                _unitOfWork.Auhtor.Add(unknownAuthor);
                _unitOfWork.Save();
            }
            
            int unknownAuthorId = _unitOfWork.Auhtor.Get(c => c.Name == "Unknown").Id;

            if (authorId == unknownAuthorId)
            {
                TempData["error"] = $"\"Unknown\" author cannot be deleted";
                return RedirectToAction("Index");
            }

            foreach (var book in BooksWithThatAuthor)
            {
                book.AuthorId = unknownAuthorId;
                _unitOfWork.Book.Update(book);
                _unitOfWork.Save();
            }


            Author authorToDelete = _unitOfWork.Auhtor.Get(a => a.Id == authorId);
            
            if(authorToDelete == null)
                return NotFound();

            _unitOfWork.Auhtor.Remove(authorToDelete);
            _unitOfWork.Save();

            TempData["success"] = $"Author \"{authorToDelete.Name + " " + authorToDelete.Surname}\" has been deleted successfully";
            return RedirectToAction("Index", "Author");
        }

        public IActionResult Upsert(int? authorId)
        {
            RegionInfo[] countries = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .Select(culture => new RegionInfo(culture.Name))
                    .DistinctBy(region => region.Name).OrderBy(x => x.EnglishName)
                    .ToArray();

            IEnumerable<SelectListItem> countryList = countries.Select(c => new SelectListItem
            {
                Text = c.EnglishName,
                Value = c.TwoLetterISORegionName
            });

            AuthorVM authorVM = new AuthorVM()
            {
                CountryList = countryList,
                AuthorBooksList = _unitOfWork.Book.GetAll(b => b.AuthorId == authorId, includeProperties: "Author,Category").ToList()
            };

            if (authorId == 0 || authorId == null)
            {
                authorVM.Author = new Author();
                
                return View(authorVM);
            }
            else
            {
                authorVM.Author = _unitOfWork.Auhtor.Get(a => a.Id == authorId);

                return View(authorVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(AuthorVM authorVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if(file == null && authorVM.Author.Id == 0)
                {
                    return View(authorVM);
                }

                if (authorVM.Author.Id == 0)
                {
                    _unitOfWork.Auhtor.Add(authorVM.Author);
                    _unitOfWork.Save();
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"images\authors\author-" + authorVM.Author.Id;
                    string finalPath = Path.Combine(wwwRootPath, productPath); 

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    authorVM.Author.ImageUrl = @"\" + productPath + @"\" + fileName;
                }

                _unitOfWork.Auhtor.Update(authorVM.Author);
                _unitOfWork.Save();

                TempData["success"] = $"Product \"{authorVM.Author.Name + " " + authorVM.Author.Surname }\" created/updated successfully";
                return RedirectToAction("Index", "Author");
            }
            else
            {
                TempData["error"] = $"Invalid \"{authorVM.Author.Name}\" data";

                return View(authorVM.Author);
            }
        }

        public IActionResult DeleteImage(int? authorId)
        {
            if (authorId != 0 && authorId != null)
            {
                Author authorFromDb = _unitOfWork.Auhtor.Get(a => a.Id == authorId);
                string? imageToDeleteUrl = Path.Combine(_webHostEnvironment.WebRootPath, authorFromDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(imageToDeleteUrl))
                    System.IO.File.Delete(imageToDeleteUrl);

                authorFromDb.ImageUrl = "";
                _unitOfWork.Auhtor.Update(authorFromDb);
                _unitOfWork.Save();

                TempData["success"] = $"Image for \"{authorFromDb.Name + " " + authorFromDb.Surname}\" has been deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { authorId = authorId });
        }

        public IActionResult DeleteAll(int authorId)
        {
            var booksToRemove = _unitOfWork.Book.GetAll(b => b.AuthorId == authorId);
            _unitOfWork.Book.RemoveRange(booksToRemove);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Upsert), new { authorId = authorId });
        }
    }
}
