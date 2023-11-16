using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Course_work.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Course_work.Areas.Admin.Controllers
{
    // Клас-Контролер замовлення
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Метод завантаження сторінки зі всіма замовленнями
        public IActionResult Index()
        {
            List<OrderVM> orders = new List<OrderVM>();

            var allOrderHeaders = _unitOfWork.OrderHeader.GetAll().ToList();

            foreach (var orderHeader in allOrderHeaders)
            {
                OrderVM orderVm = new OrderVM()
                {
                    OrderHeader = orderHeader,
                    orderDetail = _unitOfWork.OrderDetail.GetAll(d => d.OrderHeaderId == orderHeader.Id).ToList()
                };

                orders.Add(orderVm);
            }

            return View(orders);
        }

        // Метод для оновлення інормації про замовлення
        public IActionResult UpdateOrderDetail(OrderVM OrderVM)
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["success"] = "Order details have been updated successfully";

            return RedirectToAction(nameof(Details), new { orderHeaderId = orderHeaderFromDb.Id });
        }

        // Метод для видалення замовлення
        public IActionResult Delete(int? orderHeaderId)
        {
            if (orderHeaderId == 0 || orderHeaderId == null)
                return NotFound();

            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(b => b.Id == orderHeaderId);

            if (orderHeader == null)
                return NotFound();

            if (orderHeader.OrderStatus != "Shipped")
                TempData["error"] = "You can not delete order because it does not have status \"Shipped\"";

            _unitOfWork.OrderHeader.Remove(orderHeader);
            _unitOfWork.Save();

            TempData["success"] = $"Order with tracking number \"{orderHeader.TrackingNumber}\" has been deleted successfully";
            return RedirectToAction("Index", "Order");
        }

        // Завантаження інормації про замовлення
        public IActionResult Details(int? orderHeaderId)
        {
            if (orderHeaderId == 0 || orderHeaderId == null)
                return NotFound();

            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(b => b.Id == orderHeaderId),
            };

            orderVM.orderDetail = _unitOfWork.OrderDetail.GetAll(d => d.OrderHeaderId == orderHeaderId, includeProperties:"Book").ToList();

            return View(orderVM);
        }

        // Pміна статусу замовлення на "Processing"
        [HttpPost]
        public IActionResult StartProcessing(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(b => b.Id == orderVM.OrderHeader.Id);

            orderHeader.OrderStatus = "Processing";
            _unitOfWork.OrderHeader.Update(orderHeader);

            _unitOfWork.Save();

            TempData["success"] = "Order details have been updated successfully";

            return RedirectToAction(nameof(Details), new { orderHeaderId = orderVM.OrderHeader.Id });
        }

        // Pміна статусу замовлення на "Shipped"
        [HttpPost]
        public IActionResult ShipOrder(OrderVM orderVM)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(b => b.Id == orderVM.OrderHeader.Id);
            orderHeader.OrderStatus = "Shipped";

            orderHeader.ShippingDate = DateTime.Now;

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();

            TempData["success"] = "Order has been shipped successfully";

            return RedirectToAction(nameof(Details), new { orderHeaderId = orderVM.OrderHeader.Id });
        }
    }
}
