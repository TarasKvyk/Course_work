using BookStore.Models;

namespace BookStore.Models.ViewModels
{
    public class ShoppingCartVM
    {
        // View model для передавання на view кількох об'єктів разом із OrderHeader для здійснення замовлення
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
