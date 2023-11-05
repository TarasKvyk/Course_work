using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BookStore.Models.ViewModels;
using Course_work.Models;
using System.Globalization;

namespace Course_work.Areas.Customer.Controllers
{
    //[Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Book> bookList = _unitOfWork.Book.GetAll().ToList();

            ViewBag.CartNumber = GetCartCount();

            return View(bookList);
        }

        private int GetCartCount()
        {
            return _unitOfWork.ShoppingCart.GetAll().Count();
        }

        public IActionResult Details(int bookId)
        {
            var bookFromDb = _unitOfWork.Book.Get(b => b.Id == bookId, includeProperties:"Category,Author");

            ShoppingCart ShoppingCart = new ShoppingCart()
            {
                Book = bookFromDb,
                Count = 1,
                Price = bookFromDb.Price,
                BookId = bookId
            };

            ViewBag.CartNumber = GetCartCount();
            return View(ShoppingCart);
        }

        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            // var claimsIdentity = (ClaimsIdentity)User.Identity; // Отримуємо Id користувач, що зараз в акаунті
            // var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            // shoppingCart.ApplicationUserId = userId;
            // 
            // ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId && x.ProductId == shoppingCart.ProductId);

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.BookId == shoppingCart.BookId);

            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                // updating product count UI
            }

            TempData["success"] = $"Cart updated successfully";

            return RedirectToAction("Index");
        }

        public IActionResult AuthorDetails(int authorId)
        {
            var authorFromDb = _unitOfWork.Auhtor.Get(a => a.Id == authorId);

            RegionInfo regionInfo = new RegionInfo(authorFromDb.Country);
            string countryName = regionInfo.EnglishName;

            AuthorVM authorVM = new AuthorVM()
            {
                Author = authorFromDb,
                Country = countryName
            };

            ViewBag.CartNumber = GetCartCount();
            return View(authorVM);
        }

        public IActionResult Privacy()
        {
            ViewBag.CartNumber = GetCartCount();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.CartNumber = GetCartCount();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}