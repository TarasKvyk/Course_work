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
    }
}
