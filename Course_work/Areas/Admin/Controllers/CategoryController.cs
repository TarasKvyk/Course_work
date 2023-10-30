using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Unility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                    CategoryNames = categoryNames,
                    Children = new ChildrenCategory(),
                    Fiction = new FictionCategory(),
                    Dictionary = new DictionaryCategory(),
                    History = new HistoryCategory(),
                    Scientific = new ScientificCategory()
                };

                return View(CategoryVM1);
            }

            Category category = _unitOfWork.Category.Get(c => c.Id == categoryId);

            CategoryVM CategoryVM = new CategoryVM()
            {
                Category = category,
                CategoryNames = categoryNames,
                Children = new ChildrenCategory(),
                Fiction = new FictionCategory(),
                Dictionary = new DictionaryCategory(),
                History = new HistoryCategory(),
                Scientific = new ScientificCategory()
            };

            return View(CategoryVM);
        }


        private void CopyCategoryValues(Category sourceCategory, Category destinationCategory)
        {
            if (sourceCategory == null || destinationCategory == null)
                return;

            destinationCategory.Name = sourceCategory.Name;
            destinationCategory.KeyWords = sourceCategory.KeyWords;
            destinationCategory.Specialization = sourceCategory.Specialization;
            destinationCategory.CategoryDescrition = sourceCategory.CategoryDescrition;
            destinationCategory.Id = sourceCategory.Id;
        }

        [HttpPost]
        public IActionResult Upsert(CategoryVM CategoryVM, string selectedCategory)
        {
            if (ModelState.IsValid)
            {
                if (CategoryVM.Category.Id == 0)
                {
                    Category? categoryToAdd = null;

                    if (!string.IsNullOrEmpty(CategoryVM.History.Period))
                    {
                        categoryToAdd = new HistoryCategory();
                        CopyCategoryValues(CategoryVM.Category, categoryToAdd);
                        ((HistoryCategory)categoryToAdd).Period = CategoryVM.History.Period;
                    }
                    else if (!string.IsNullOrEmpty(CategoryVM.Dictionary.IntoLanguage) && !string.IsNullOrEmpty(CategoryVM.Dictionary.NativeLanguage))
                    {
                        categoryToAdd = new DictionaryCategory();
                        CopyCategoryValues(CategoryVM.Category, categoryToAdd);
                        ((DictionaryCategory)categoryToAdd).NativeLanguage = CategoryVM.Dictionary.NativeLanguage;
                        ((DictionaryCategory)categoryToAdd).IntoLanguage = CategoryVM.Dictionary.IntoLanguage;
                    }
                    else if (!string.IsNullOrEmpty(CategoryVM.Fiction.LiteraryFormat))
                    {
                        categoryToAdd = new FictionCategory();
                        CopyCategoryValues(CategoryVM.Category, categoryToAdd);
                        ((FictionCategory)categoryToAdd).LiteraryFormat = CategoryVM.Fiction.LiteraryFormat;
                    }
                    else if (!string.IsNullOrEmpty(CategoryVM.Children.PurposeAge))
                    {
                        categoryToAdd = new ChildrenCategory();
                        CopyCategoryValues(CategoryVM.Category, categoryToAdd);
                        ((ChildrenCategory)categoryToAdd).PurposeAge = CategoryVM.Children.PurposeAge;
                    }
                    else if (!string.IsNullOrEmpty(CategoryVM.Scientific.KnowledgeBranch))
                    {
                        categoryToAdd = new ScientificCategory();
                        CopyCategoryValues(CategoryVM.Category, categoryToAdd);
                        ((ScientificCategory)categoryToAdd).KnowledgeBranch = CategoryVM.Scientific.KnowledgeBranch;
                    }

                    if (categoryToAdd != null)
                    {
                        _unitOfWork.Category.Add(categoryToAdd);
                        _unitOfWork.Save();
                    }
                }
                else
                {
                    // Виконати логіку для оновлення існуючої категорії
                    // Ви можете використовувати значення selectedCategory
                }

                return RedirectToAction("Index", "Category");
                // Після виконання логіки перенаправити користувача на іншу сторінку або вивести повідомлення
            }

            // Якщо виникла помилка, повернути користувача назад на форму з помилками
            return View(CategoryVM);
        }
    }
}
