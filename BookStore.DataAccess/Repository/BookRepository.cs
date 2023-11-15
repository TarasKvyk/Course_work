using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Book entity)
        {
            Book? bookFromDb = _db.Books.FirstOrDefault(x => x.Id == entity.Id);

            if(bookFromDb != null) {
                bookFromDb.Price = entity.Price;
                bookFromDb.Year = entity.Year;
                bookFromDb.AvailableCount = entity.AvailableCount;
                bookFromDb.PageCount = entity.PageCount;
                if (bookFromDb.AuthorId != entity.AuthorId)
                {
                    bookFromDb.AuthorId = entity.AuthorId;
                    bookFromDb.Author = _db.Authors.FirstOrDefault(c => c.Id == entity.AuthorId);
                }

                bookFromDb.Description = entity.Description;
                bookFromDb.Language = entity.Language;
                bookFromDb.Title = entity.Title;

                if(entity.ImageUrl != null)
                    bookFromDb.ImageUrl = entity.ImageUrl;

                if (bookFromDb.CategoryId != entity.CategoryId)
                {
                    bookFromDb.CategoryId = entity.CategoryId;
                    bookFromDb.Category = _db.Categories.FirstOrDefault(c => c.Id == entity.CategoryId);
                }
            }
        }
    }
}
