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
	//[Area("Customer")]
	//[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

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

        public IActionResult Plus(int cardId)
		{
			ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == cardId);
          
			cartFromDb.Count++;

            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cardId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == cardId);

            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cardId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == cardId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();


            TempData["success"] = $"Cart has been removed";

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Summary()
        {
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(includeProperties: "Book"),
                OrderHeader = new OrderHeader()
            };

			if (ShoppingCartVM.ShoppingCartList == null || !ShoppingCartVM.ShoppingCartList.Any())
			{
                TempData["error"] = $"Your Cart is Empty";
				
				return RedirectToAction(nameof(Index));
            }


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = cart.Book.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST(ShoppingCartVM shoppingCartVM)
		{
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
			_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ShoppingCartList);
			_unitOfWork.Save();
			
			TempData["success"] = $"Order \"{ShoppingCartVM.OrderHeader.TrackingNumber}\" has been placed successfully";
			
			return RedirectToAction(nameof(Index), "Home");
		}
    }
}