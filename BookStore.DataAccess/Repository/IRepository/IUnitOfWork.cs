using BookStore.DataAccess.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IBookRepository Book { get; }
        ICategoryRepository Category { get; set; }
        IAuthorRepository Auhtor { get; set; }
        IShoppingCartRepository ShoppingCart { get; set; }
		IOrderDetailRepository OrderDetail { get; set; }
		IOrderHeaderRepository OrderHeader { get; set; }

		void Save();
    }
}
