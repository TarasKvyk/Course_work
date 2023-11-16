using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    // Клас UnitOfWork, що має доступ до всіх таблиць бази дани
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IBookRepository Book { get; set; }
        public ICategoryRepository Category { get; set; }
        public IAuthorRepository Auhtor { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }
		public IOrderDetailRepository OrderDetail { get; set; }
		public IOrderHeaderRepository OrderHeader { get; set; }

		public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Book = new BookRepository(_db);
            Category = new CategoryRepository(_db);
            Auhtor = new AuthorRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
			OrderDetail = new OrderDetailRepository(_db);
			OrderHeader = new OrderHeaderRepository(_db);
		}

        // Збереження змін
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
