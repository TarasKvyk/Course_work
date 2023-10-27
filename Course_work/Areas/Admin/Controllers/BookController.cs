using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Course_work.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Upsert(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upsert()
        {
            return View();
        }
    }
}
