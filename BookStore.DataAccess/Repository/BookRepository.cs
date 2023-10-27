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
            _db.Update(entity);
            Book bookFromDb = _db.Books.FirstOrDefault(x => x.Id == entity.Id);
            bookFromDb.Price = entity.Price;
            bookFromDb.CategoryId = entity.CategoryId;
            bookFromDb.Year = entity.Year;
            bookFromDb.AuthorId = entity.AuthorId;
            bookFromDb.Description = entity.Description;
            bookFromDb.Language = entity.Language;
            bookFromDb.Title = entity.Title;
            bookFromDb.ImageUrl = entity.ImageUrl;
            bookFromDb.CategoryId = entity.CategoryId;

            //if (bookFromDb != null)
            //{
            //    if (bookFromDb.Category is HistoryCategory)
            //    {
            //        HistoryCategory categoryFromDb = (HistoryCategory)bookFromDb.Category;
            //        HistoryCategory newCategory = (HistoryCategory)entity.Category;

            //        categoryFromDb.Period = newCategory.Period;

            //    }
            //    else if (bookFromDb.Category is FictionCategory)
            //    {
            //        FictionCategory categoryFromDb = (FictionCategory)bookFromDb.Category;
            //        FictionCategory newCategory = (FictionCategory)entity.Category;

            //        categoryFromDb.LiteraryFormat = newCategory.LiteraryFormat;

            //    }
            //    else if(bookFromDb.Category is DictionaryCategory)
            //    {
            //        DictionaryCategory categoryFromDb = (DictionaryCategory)bookFromDb.Category;
            //        DictionaryCategory newCategory = (DictionaryCategory)entity.Category;

            //        categoryFromDb.NativeLanguage = newCategory.NativeLanguage;
            //        categoryFromDb.IntoLanguage = newCategory.IntoLanguage;
            //    }
            //    else if(bookFromDb.Category is ScientificCategory)
            //    {
            //        ScientificCategory categoryFromDb = (ScientificCategory)bookFromDb.Category;
            //        ScientificCategory newCategory = (ScientificCategory)entity.Category;

            //        categoryFromDb.KnowledgeBranch = newCategory.KnowledgeBranch;

            //    }
            //    else if(bookFromDb.Category is ChildrenCategory)
            //    {
            //        ChildrenCategory categoryFromDb = (ChildrenCategory)bookFromDb.Category;
            //        ChildrenCategory newCategory = (ChildrenCategory)entity.Category;

            //        categoryFromDb.PurposeAge = newCategory.PurposeAge;
            //    }
            //}
        }
    }
}
