using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Unility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Course_work.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();

            return View(categories);
        }

        public IActionResult Delete(int? categoryId)
        {
            if (categoryId == 0 || categoryId == null)
                return NotFound();

            var BooksWithThatCategory = _unitOfWork.Book.GetAll(b => b.CategoryId == categoryId, includeProperties:"Category,Author", false).ToList();

            if (_unitOfWork.Category.Get(c => c.Name == "Unknown") == null)
            {
                Category unknownCategory = new Category()
                {
                    Id = 0,
                    Name = "Unknown",
                    Specialization = "None",
                    KeyWords = "",
                    CategoryDescrition = "None",
                };

                _unitOfWork.Category.Add(unknownCategory);
                _unitOfWork.Save();
            }

            int unknownCategoryId = _unitOfWork.Category.Get(c => c.Name == "Unknown").Id;

            if (categoryId == unknownCategoryId)
            {
                TempData["error"] = $"\"Unknown\" category cannot be deleted";
                return RedirectToAction("Index");
            }

            foreach (var book in BooksWithThatCategory)
            {
                book.CategoryId = unknownCategoryId;
                _unitOfWork.Book.Update(book);
                _unitOfWork.Save();
            }

            Category categoryToDelete = _unitOfWork.Category.Get(a => a.Id == categoryId);

            if (categoryToDelete == null)
                return NotFound();

            _unitOfWork.Category.Remove(categoryToDelete);
            _unitOfWork.Save();

            TempData["success"] = $"Category \"{categoryToDelete.Name + " " + categoryToDelete.Specialization}\" has been deleted successfully";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Upsert(int? categoryId)
        {
            if (_unitOfWork.Category.Get(c => c.Name == "Unknown") == null)
            {
                Category unknownCategory = new Category()
                {
                    Id = 0,
                    Name = "Unknown",
                    Specialization = "None",
                    KeyWords = "",
                    CategoryDescrition = "None",
                };

                _unitOfWork.Category.Add(unknownCategory);
                _unitOfWork.Save();
            }

            int unknownCategoryId = _unitOfWork.Category.Get(c => c.Name == "Unknown").Id;

            if (categoryId == unknownCategoryId)
            {
                TempData["error"] = $"\"Unknown\" category cannot be edited";
                return RedirectToAction("Index");
            }

            List<string> categoryList = new List<string>();
            var constantCategoryValues = ConstCategoryDetails.GetCategoryValues().ToList();

            for (int i = 0; i < constantCategoryValues.Count; i++)
            {
                categoryList.Add(constantCategoryValues[i].Item2);
            }

            IEnumerable<SelectListItem> categoryNames = categoryList.Select(c => new SelectListItem
            {
                Text = c.ToString().Replace("Category", ""),
                Value = c.ToString()
            });

            if (categoryId == null || categoryId == 0)
            {
                CategoryVM CategoryVM1 = new CategoryVM()
                {
                    Category = new Category(),
                    CategoryNames = categoryNames
                };

                return View(CategoryVM1);
            }

            Category category = _unitOfWork.Category.Get(c => c.Id == categoryId);

            CategoryVM CategoryVM = new CategoryVM()
            {
                Category = category,
                CategoryNames = categoryNames
            };

            return View(CategoryVM);
        }


    }
}
