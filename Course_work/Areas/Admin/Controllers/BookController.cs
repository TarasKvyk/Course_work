using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Course_work.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IUnitOfWork unitOfWork) // dependency injection
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Book> bookList = _unitOfWork.Book.GetAll(includeProperties: "Category,Author").ToList();

            return View(bookList);
        }

        public IActionResult Upsert(int? bookId)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
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
                CategoryList = categoryList
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
        public IActionResult Upsert()
        {
            return View();
        }
    }
}
