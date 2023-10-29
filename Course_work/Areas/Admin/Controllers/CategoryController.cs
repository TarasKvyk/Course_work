using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Unility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Course_work.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _UnitOfWork.Category.GetAll().ToList();

            return View(categories);
        }

        public IActionResult Upsert(int? categoryId)
        {
            IEnumerable<string> categoryList = ConstCategoryDetails.CategoryNames;

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

            Category category = _UnitOfWork.Category.Get(c => c.Id == categoryId);

            CategoryVM CategoryVM = new CategoryVM()
            {
                Category = category,
                CategoryNames = categoryNames
            };

            return View(CategoryVM);
        }


    }
}
