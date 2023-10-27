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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category entity)
        {
            _db.Update(entity);
            Category categoryFromDb = _db.Categories.FirstOrDefault(x => x.Id == entity.Id);

            if(categoryFromDb != null)
            {
                if (categoryFromDb is HistoryCategory)
                {
                    HistoryCategory category = (HistoryCategory)categoryFromDb;
                    HistoryCategory newCategory = (HistoryCategory)entity;

                    category.Period = newCategory.Period;

                }
                else if (categoryFromDb is FictionCategory)
                {
                    FictionCategory category = (FictionCategory)categoryFromDb;
                    FictionCategory newCategory = (FictionCategory)entity;

                    category.LiteraryFormat = newCategory.LiteraryFormat;

                }
                else if(categoryFromDb is DictionaryCategory)
                {
                    DictionaryCategory category = (DictionaryCategory)categoryFromDb;
                    DictionaryCategory newCategory = (DictionaryCategory)entity;

                    category.NativeLanguage = newCategory.NativeLanguage;
                    category.IntoLanguage = newCategory.IntoLanguage;
                }
                else if(categoryFromDb is ScientificCategory)
                {
                    ScientificCategory category = (ScientificCategory)categoryFromDb;
                    ScientificCategory newCategory = (ScientificCategory)entity;

                    category.KnowledgeBranch = newCategory.KnowledgeBranch;
                }
                else if(categoryFromDb is ChildrenCategory)
                {
                    ChildrenCategory category = (ChildrenCategory)categoryFromDb;
                    ChildrenCategory newCategory = (ChildrenCategory)entity;

                    category.PurposeAge = newCategory.PurposeAge;
                }
            }
        }
    }
}
