using Data.Database;
using Data.Repository.Abstract;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public new void Save()
        {
            _db.SaveChanges();
        }

        public new void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
