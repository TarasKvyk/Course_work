using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Course_work.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert()
        {
            List<Book> bookList = new List<Book>();
            return View();
        }
    }
}
