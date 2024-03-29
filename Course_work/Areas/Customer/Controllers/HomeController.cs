﻿using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookStore.Models.ViewModels;
using Course_work.Models;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Unility;
using Newtonsoft.Json;

namespace Course_work.Areas.Customer.Controllers
{
    // Клас-Контролер для головної сторінки
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public static string SeachQueryResultJson { get; private set; } = string.Empty;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Метод завантаження головної сторінки 
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            { 
                AuthorList = GetAuthorSelectList(),
                OrderOptionsList = GetOrderOptionsList(),
                AvailableLanguages = GetAvailableAuthorBookLanguages(0),
                AvailableCategories = GetAvailableAuthorCategories(0).OrderBy(c => c.Name).ThenBy(c => c.Specialization).ToList()
            };

            // Відображення лише 6 книг на сторінці
            homeVM.BookList = _unitOfWork.Book.GetAll(includeProperties: "Author,Category")
                .Skip((homeVM.CurrentPageNumber - 1) * homeVM.BooksPerPage)
                .Take(homeVM.BooksPerPage)
                .ToList();

            int allBookCount = _unitOfWork.Book.GetAll().Count();
            homeVM.PageNumber = allBookCount / homeVM.BooksPerPage;

            if (allBookCount % homeVM.BooksPerPage != 0)
                homeVM.PageNumber++;

            return View(homeVM);
        }

        // Метод для відображення книг певного автора з фільтрами
        public IActionResult AuthorBooks(HomeVM homeVM, int currentPageNumber = 1)
        {
            homeVM.AuthorList = GetAuthorSelectList();
            int minPrice = homeVM.MinPrice;
            int maxPrice = homeVM.MaxPrice;

            if ((minPrice < 0 || minPrice > 10000) || (maxPrice < 0 || maxPrice > 10000))
            {
                TempData["error"] = $"Price must be in the range from 0 to 10000";
                return RedirectToAction("Index");
            }
            else if (minPrice >= maxPrice)
            {
                TempData["error"] = $"Min Price must be smaller than Max Price";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(SeachQueryResultJson))
            {
                List<Book> pageBookList;
                
                if (homeVM.AuthorId <= 0)
                {
                    // Якщо не вибрано автора
                    pageBookList = _unitOfWork.Book.GetAll(includeProperties: "Author,Category").ToList();
                }
                else
                {
                    pageBookList = _unitOfWork.Book.GetAll(b => b.AuthorId == homeVM.AuthorId, includeProperties: "Author,Category").ToList();
                }

                if (homeVM.ChosenLanguages != null && homeVM.ChosenLanguages.Count > 0)
                {
                    // Фільтр за мовою
                    pageBookList = pageBookList.Where(b => homeVM.ChosenLanguages.Contains(b.Language)).ToList();
                }

                if (homeVM.ChosenCategoryIds != null && homeVM.ChosenCategoryIds.Count > 0)
                {
                    // Фільтр за категорією
                    pageBookList = pageBookList.Where(b => homeVM.ChosenCategoryIds.Contains(b.CategoryId.GetValueOrDefault())).ToList();
                }

                pageBookList = pageBookList.Where(b => b.Price >= minPrice && b.Price <= maxPrice).ToList();

                homeVM.BookList = pageBookList;
            }
            else
            {
                // Отримання HomeVM із десеріалізованої стрічки
                HomeVM _homeVM = JsonConvert.DeserializeObject<HomeVM>(SeachQueryResultJson);

                if (homeVM.AuthorId <= 0)
                {
                    // Якщо не вибрано автора
                    homeVM.BookList = _homeVM.BookList
                        .Where(b => b.Price >= minPrice && b.Price <= maxPrice)
                        .ToList();
                }
                else
                {
                    homeVM.BookList = _homeVM.BookList
                        .Where(b => (b.AuthorId == homeVM.AuthorId) && (b.Price >= minPrice && b.Price <= maxPrice))
                        .ToList();
                }
            
                if (homeVM.ChosenLanguages != null && homeVM.ChosenLanguages.Count != 0)
                {
                    // Фільтр за мовою
                    homeVM.BookList = homeVM.BookList
                        .Where(b => homeVM.ChosenLanguages.Contains(b.Language))
                        .ToList();
                }

                if (homeVM.ChosenCategoryIds != null && homeVM.ChosenCategoryIds.Count != 0)
                {
                    // Фільтр за категорією
                    homeVM.BookList = homeVM.BookList.Where(b => homeVM.ChosenCategoryIds.Contains(b.CategoryId.GetValueOrDefault())).ToList();
                }

                homeVM.SearchQuery = _homeVM.SearchQuery;
            }

            // Фільтр порядок відображення на сторінці
            switch (homeVM.OrderOptionId)
            {
                case 1:
                    homeVM.BookList = homeVM.BookList.OrderBy(b => b.Title).ToList();
                    break;
                case 2:
                    homeVM.BookList = homeVM.BookList.OrderByDescending(b => b.Title).ToList();
                    break;
                case 3:
                    homeVM.BookList = homeVM.BookList.OrderBy(b => b.Price).ToList();
                    break;
                case 4:
                    homeVM.BookList = homeVM.BookList.OrderByDescending(b => b.Price).ToList();
                    break;
                case 5:
                    homeVM.BookList = homeVM.BookList.OrderByDescending(b => b.Year).ToList();
                    break;
                default:
                    break;
            }

            // блок коду для відображення певної сторінки, де максимум 6 книг
            homeVM.CurrentPageNumber = currentPageNumber;
            int allBookCount = homeVM.BookList.Count;
            homeVM.PageNumber = allBookCount / homeVM.BooksPerPage;
            if (allBookCount % homeVM.BooksPerPage != 0)
                homeVM.PageNumber++;

            homeVM.BookList = homeVM.BookList
                .Skip((currentPageNumber - 1) * homeVM.BooksPerPage)
                .Take(homeVM.BooksPerPage)
                .ToList();

            // формування HomeVM
            homeVM.AvailableLanguages = GetAvailableAuthorBookLanguages(homeVM.AuthorId);
            homeVM.AvailableCategories = GetAvailableAuthorCategories(homeVM.AuthorId).OrderBy(c => c.Name).ThenBy(c => c.Specialization).ToList();
            homeVM.OrderOptionsList = GetOrderOptionsList();

            return View("Index", homeVM);
        }

        // Метод очищення пошуку за словами
        public IActionResult ClearSearch(HomeVM homeVM)
        {
            SeachQueryResultJson = string.Empty;

            return RedirectToAction("AuthorBooks", homeVM);
        }
        
        //Метод пошуку за словами
        public IActionResult Search(string searchQuery)
        {
            List<Book> bookList = _unitOfWork.Book.GetAll(includeProperties: "Author,Category").ToList();

            HomeVM homeVM = new HomeVM()
            {
                AuthorList = GetAuthorSelectList(),
                BookList = new List<Book>(),
                SearchQuery = searchQuery
            };

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                string[] searchTerms = searchQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                searchTerms = searchTerms.Where(s => s.Length > 3).ToArray();

                if(searchTerms == null || searchTerms.Length < 1)
                {
                    TempData["warning"] = $"Query must contain word with length higher than 3";
                    return RedirectToAction(nameof(Index));
                }

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
                             b.Author.Surname.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                             b.Category.KeyWords.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                             b.Category.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                             b.Category.Specialization.Contains(term, StringComparison.OrdinalIgnoreCase)
                             )
                        )
                    ).Except(bookFilteredList)
                );

                homeVM.BookList.AddRange(bookFilteredList);
            }

            SeachQueryResultJson = JsonConvert.SerializeObject(homeVM);

            return RedirectToAction(nameof(AuthorBooks), homeVM);
        }

        //Метод для отримання категорій книг певного автора
        private List<Category?> GetAvailableAuthorCategories(int authorId)
        {
            var authorBooks = authorId > 0
                ? _unitOfWork.Book.GetAll(b => b.AuthorId == authorId, includeProperties: "Author,Category").ToList()
                : _unitOfWork.Book.GetAll(includeProperties: "Author,Category").ToList();

            return authorBooks
                .Select(book => book.Category)
                .DistinctBy(c => c.Id)
                .ToList();
        }

        //Метод для отримання мов написання книг певного автора
        private List<string> GetAvailableAuthorBookLanguages(int authorId)
        {
            var authorBooks = authorId > 0
                ? _unitOfWork.Book.GetAll(b => b.AuthorId == authorId, includeProperties: "Author,Category").ToList()
                : _unitOfWork.Book.GetAll(includeProperties: "Author,Category").ToList();

            return authorBooks
                .GroupBy(book => book.Language)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .ToList();
        }

        //Метод для отримання авторів 
        private IEnumerable<SelectListItem> GetAuthorSelectList()
        {
            List<Author> authorList = _unitOfWork.Auhtor.GetAll(a => a.Name != "Unknown").ToList();
            authorList.Add(new Author { Id = -1, Name = "All", Surname = "" });

            IEnumerable<SelectListItem> authorListNames = authorList.Select(c => new SelectListItem
            {
                Text = c.Name + " " + c.Surname,
                Value = c.Id.ToString()
            });

            return authorListNames.OrderBy(a => a.Text);
        }

        //Метод для отримання способів відображення книжок
        private IEnumerable<SelectListItem> GetOrderOptionsList()
        {
            List<Author> authorList = _unitOfWork.Auhtor.GetAll(a => a.Name != "Unknown").ToList();
            authorList.Add(new Author { Id = -1, Name = "All", Surname = "" });

            IEnumerable<SelectListItem> authorListNames = BookOrderOptions.GetOrderOptions().Select(c => new SelectListItem
            {
                Value = c.Item1.ToString(),
                Text = c.Item2
            });

            return authorListNames;
        }

        //Метод відривання сторінки з деталями книги
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

            return View(ShoppingCart);
        }

        //Метод створення замовлення
        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.BookId == shoppingCart.BookId, includeProperties: "Book");

            // Перевірка на допустимі значення кількості товару
            if(shoppingCart.Count <= 0)
            {
                TempData["warning"] = $"Count must be greater than zero";
                return RedirectToAction("Details", new { bookId = shoppingCart.BookId });
            }

            // Якщо корзини немає ще
            if (cartFromDb != null)
            {
                // Перевірка на допустимі значення кількості товару
                if (shoppingCart.Count > cartFromDb.Book.AvailableCount)
                {
                    TempData["warning"] = $"Available book count is " + cartFromDb.Book.AvailableCount;
                    return RedirectToAction("Details", new { bookId = shoppingCart.BookId });
                }

                cartFromDb.Count += shoppingCart.Count;
                cartFromDb.Book.AvailableCount -= shoppingCart.Count;
                _unitOfWork.Book.Update(cartFromDb.Book);
                _unitOfWork.Save();

                cartFromDb.Book = null;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                // Якщо корзини вже є

                Book bookFromDb = _unitOfWork.Book.Get(b => b.Id == shoppingCart.BookId);

                if (shoppingCart.Count > bookFromDb.AvailableCount)
                {
                    TempData["warning"] = $"Available book count is " + bookFromDb.AvailableCount;
                    return RedirectToAction("Details", new { bookId = shoppingCart.BookId });
                }

                shoppingCart.Price = bookFromDb.Price;
                bookFromDb.AvailableCount -= shoppingCart.Count;

                _unitOfWork.Book.Update(bookFromDb);
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
            }

            TempData["success"] = $"Cart updated successfully";

            return RedirectToAction("Index");
        }

        // Метод для відображення інормації про автора
        public IActionResult AuthorDetails(int authorId, int bookId)
        {
            var authorFromDb = _unitOfWork.Auhtor.Get(a => a.Id == authorId);

            RegionInfo regionInfo = new RegionInfo(authorFromDb.Country);
            string countryName = regionInfo.EnglishName;

            AuthorVM authorVM = new AuthorVM()
            {
                Author = authorFromDb,
                Country = countryName,
                AuthorBooksList = _unitOfWork.Book.GetAll(b => b.AuthorId == authorId, includeProperties:"Author,Category").ToList(),
                BookIdToRedirect = bookId
            };

            return View(authorVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}