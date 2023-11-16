    using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Unility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BooksWeb.Areas.Customer.Controllers
{
    // Клас-Контролер корзини
    public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM? ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

        // Метод завантаження сторінки корзини
        public IActionResult Index()
		{
            ViewBag.CartNumber = _unitOfWork.ShoppingCart.GetAll().Count();

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(includeProperties: "Book"),
                OrderHeader = new OrderHeader()
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = cart.Book.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
		}

        // Метод збільшення кількості товару на 1
        public IActionResult Plus(int cardId)
		{
			ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == cardId, includeProperties:"Book");
          
            if(cartFromDb.Count < cartFromDb.Book.AvailableCount)
            {
                cartFromDb.Count++;
                cartFromDb.Book.AvailableCount--;
                _unitOfWork.Book.Update(cartFromDb.Book);
            }
            else
            {
                TempData["warning"] = $"More books are not available";
            }

            cartFromDb.Book = null;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // Метод зменшення кількості товару на 1
        public IActionResult Minus(int cardId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == cardId, includeProperties: "Book");

            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                cartFromDb.Book.AvailableCount++;
                _unitOfWork.Book.Update(cartFromDb.Book);
                cartFromDb.Book = null;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        // Метод видалення товару з корзини
        public IActionResult Remove(int cardId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == cardId, includeProperties: "Book");

            cartFromDb.Book.AvailableCount += cartFromDb.Count;
            _unitOfWork.Book.Update(cartFromDb.Book);
            cartFromDb.Book = null;
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();

            TempData["success"] = $"Cart has been removed";

            return RedirectToAction(nameof(Index));
        }

        // Метод для переходу на сторінку оформлення замовлення
        public IActionResult Summary()
        {
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(includeProperties: "Book"),
                OrderHeader = new OrderHeader()
            };

			if (ShoppingCartVM.ShoppingCartList == null || !ShoppingCartVM.ShoppingCartList.Any())
			{
                TempData["warning"] = $"Your Cart is Empty";
                
				return RedirectToAction(nameof(Index));
            }

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = cart.Book.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        // Метод для зберігання оформленого замовлення
        [HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST(ShoppingCartVM shoppingCartVM)
        {
            if (string.IsNullOrEmpty(ShoppingCartVM.OrderHeader.City))
			{
				TempData["error"] = $"Enter correct data";
				return RedirectToAction("Summary");
            }

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(includeProperties: "Book");

			ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ShippingDate = DateTime.Now.AddDays(7);
            ShoppingCartVM.OrderHeader.OrderStatus = "Approved";

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = cart.Book.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			_unitOfWork.Save();

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				OrderDetail orderDetail = new OrderDetail()
				{
					BookId = cart.BookId,
					OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
					Price = cart.Price,
					Count = cart.Count
				};

				_unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.Save();
			}

			ShoppingCartVM.OrderHeader.TrackingNumber = ShoppingCartVM.OrderHeader.Id.ToString();
            // Видалення корзин з бази даних
			_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ShoppingCartList);
			_unitOfWork.Save();
			
			TempData["success"] = $"Order \"{ShoppingCartVM.OrderHeader.TrackingNumber}\" has been placed successfully";
			
			return RedirectToAction(nameof(Index), "Home");
		}
    }
}