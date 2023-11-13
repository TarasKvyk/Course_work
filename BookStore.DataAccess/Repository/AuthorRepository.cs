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
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext _db;

        public AuthorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Author entity)
        {
            Author authorFromDb = _db.Authors.FirstOrDefault(x => x.Id == entity.Id);

            if (authorFromDb != null)
            {
                authorFromDb.Surname = entity.Surname;
                authorFromDb.Name = entity.Name;
                authorFromDb.Country = entity.Country;
                authorFromDb.Biography = entity.Biography;
                authorFromDb.BirthDate = entity.BirthDate;

                if(!string.IsNullOrEmpty(entity.ImageUrl))
                    authorFromDb.ImageUrl = entity.ImageUrl;
            }
        }
    }
}
