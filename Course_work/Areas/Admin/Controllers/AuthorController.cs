using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

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
            if (authorId == 0 || authorId == null)
            {
                // create
                return View(new Author());
            }
            else
            {
                Author author = _unitOfWork.Auhtor.Get(a => a.Id == authorId);

                return View(author);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Author author, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if(file == null && author.Id == 0)
                {
                    return View(author);
                }

                if (author.Id == 0)
                {
                    _unitOfWork.Auhtor.Add(author);
                    _unitOfWork.Save();
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"images\authors\author-" + author.Id;
                    string finalPath = Path.Combine(wwwRootPath, productPath); 

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    author.ImageUrl = @"\" + productPath + @"\" + fileName;
                }

                _unitOfWork.Auhtor.Update(author);
                _unitOfWork.Save();

                TempData["success"] = $"Product \"{author.Name + " " + author.Surname }\" created/updated successfully";
                return RedirectToAction("Index", "Author");
            }
            else
            {
                TempData["error"] = $"Invalid \"{author.Name}\" data";

                return View(author);
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
    }
}
