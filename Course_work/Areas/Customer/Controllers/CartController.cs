using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using BookStore.Unility;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BooksWeb.Areas.Customer.Controllers
{
	//[Area("Customer")]
	//[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var shoppingCartList = _unitOfWork.ShoppingCart.GetAll(includeProperties:"Book");
			
			return View(shoppingCartList);
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
    }
}
