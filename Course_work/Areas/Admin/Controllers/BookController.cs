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
            Book book = _unitOfWork.Book.Get(b => b.Id == bookId, includeProperties:"Category,Author");

            //IEnumerable<SelectListItem> CategoryList = _unitOfWork

            BookVM bookVM = new BookVM()
            {
                
            };

            return View();
        }

        [HttpPost]
        public IActionResult Upsert()
        {
            return View();
        }
    }
}
