using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Unility;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using BookStore.Models.ViewModels;
using BookStore.Models;

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
                cart.Price = cart.Book.Price * cart.Count;
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

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Summary()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity; // Отримуємо Id користувач, що зараз в акаунті
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(/*u => u.ApplicationUserId == userId*/ includeProperties: "Book"),
                OrderHeader = new OrderHeader()
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = cart.Book.Price * cart.Count;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST(ShoppingCartVM shoppingCartVM)
		{
			//var claimsIdentity = (ClaimsIdentity)User.Identity; // Отримуємо Id користувач, що зараз в акаунті
			//var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(/*u => u.ApplicationUserId == userId, */includeProperties: "Book");

			//ApplicationUser applicationUser = _unitOfWork.ApplicationUsers.Get(u => u.Id == userId);

			ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
			//ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
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

			//if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			//{
			//	// Stripe logic
			//	var domain = "https://localhost:7033/";

			//	var options = new SessionCreateOptions
			//	{
			//		SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
			//		CancelUrl = domain + "customer/cart/index",
			//		LineItems = new List<SessionLineItemOptions>(),
			//		Mode = "payment",
			//	};

			//	foreach (var item in ShoppingCartVM.ShoppingCartList)
			//	{
			//		var SessionLineItem = new SessionLineItemOptions
			//		{
			//			PriceData = new SessionLineItemPriceDataOptions()
			//			{
			//				UnitAmount = (long)(item.Price * 100),
			//				Currency = "usd",
			//				ProductData = new SessionLineItemPriceDataProductDataOptions()
			//				{
			//					Name = item.Product.Title
			//				}
			//			},

			//			Quantity = item.Count
			//		};

			//		options.LineItems.Add(SessionLineItem);
			//	}

			//	var service = new SessionService();
			//	Session session = service.Create(options);

			//	_unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
			//	_unitOfWork.Save();

			//	Response.Headers.Add("Location", session.Url);
			//	return new StatusCodeResult(303);
			//}

			ShoppingCartVM.OrderHeader.TrackingNumber = ShoppingCartVM.OrderHeader.Id.ToString();
			_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ShoppingCartList);
			_unitOfWork.Save();
			
			TempData["success"] = $"Order \"{ShoppingCartVM.OrderHeader.TrackingNumber}\" has been placed successfully";
			
			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
