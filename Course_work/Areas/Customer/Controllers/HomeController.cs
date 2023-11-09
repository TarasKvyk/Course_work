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
using Microsoft.AspNetCore.Mvc.Rendering;

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
            HomeVM homeVM = new HomeVM() 
            { 
                AuthorList = GetAuthorSelectList(),
                BookList = _unitOfWork.Book.GetAll(includeProperties: "Author,Category").ToList()
            };

            ViewBag.CartNumber = GetCartCount();

            return View(homeVM);
        }

        public IActionResult AuthorBooks(HomeVM homeVMwithAuthorId)
        {
            if (homeVMwithAuthorId.AuthorId < 1)
                return RedirectToAction("Index");

            HomeVM homeVM = new HomeVM()
            {
                AuthorList = GetAuthorSelectList(),
                BookList = _unitOfWork.Book.GetAll(b => b.AuthorId == homeVMwithAuthorId.AuthorId, includeProperties: "Author,Category").ToList()
            };

            ViewBag.CartNumber = GetCartCount();

            return View("Index", homeVM);
        }

        private int GetCartCount()
        {
            return _unitOfWork.ShoppingCart.GetAll().Count();
        }

        //[HttpPost]
        public IActionResult Search(string searchQuery)
        {
            List<Book> bookList = _unitOfWork.Book.GetAll(includeProperties: "Author,Category").ToList();
            
            HomeVM homeVM = new HomeVM()
            {
                AuthorList = GetAuthorSelectList(),
                BookList = bookList
            };

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                string[] searchTerms = searchQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<Book> bookFilteredList = bookList.Where(b =>
                    searchTerms.Any(term =>
                        b.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(b.Description) && b.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
                    )
                ).ToList();

                bookFilteredList.AddRange(
                    bookList.Where(b =>
                        ((b.Author != null) &&
                         searchTerms.Any(term =>
                             b.Author.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                             b.Author.Surname.Contains(term, StringComparison.OrdinalIgnoreCase))
                        )
                    ).Except(bookFilteredList)
                );

                homeVM.BookList = bookFilteredList;
            }


            ViewBag.CartNumber = GetCartCount();

            return View("Index", homeVM);
        }


        private IEnumerable<SelectListItem> GetAuthorSelectList()
        {
            List<Author> authorList = _unitOfWork.Auhtor.GetAll(a => a.Name != "Unknown").ToList();
            authorList.Add(new Author { Id = -1, Name = "All", Surname = "" });

            IEnumerable<SelectListItem> authorListNames = authorList.Select(c => new SelectListItem
            {
                Text = c.Name + " " + c.Surname,
                Value = c.Id.ToString()
            });

            return authorListNames;
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
                Book bookFromDb = _unitOfWork.Book.Get(b => b.Id == shoppingCart.BookId);
                shoppingCart.Price = bookFromDb.Price;

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