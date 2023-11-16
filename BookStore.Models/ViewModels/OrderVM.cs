using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ViewModels
{
    // View model для передавання на view кількох об'єктів разом із OrderHeader для управління замовленнями
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> orderDetail { get; set; }
    }
}
