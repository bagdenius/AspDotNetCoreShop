using Data.Database;
using Data.Repository.Abstract;
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
    }
}
