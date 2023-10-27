using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Course_work.Controllers
{
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IUnitOfWork unitOfWork) // dependency injection
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Book> bookList = _unitOfWork.Book.GetAll(includeProperties: "Category").ToList();

            return View(bookList);
        }

        public IActionResult Upsert()
        {
            return View();
        }
    }
}
