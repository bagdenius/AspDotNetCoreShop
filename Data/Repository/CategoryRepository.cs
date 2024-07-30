using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(Category category)
        {
            _db.Categories.Update(category);
        }

        public override Category Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Category> query = tracked ? dbSet : dbSet.AsNoTracking();
            query = query.Where(c => c.Id == id);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties
                    .Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }
    }
}
