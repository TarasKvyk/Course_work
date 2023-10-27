using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IBookRepository Book { get; set; }
        public ICategoryRepository Category { get; set; }
        public IAuthorRepository Auhtor { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Book = new BookRepository(_db);
            Category = new CategoryRepository(_db);
            Auhtor = new AuthorRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
