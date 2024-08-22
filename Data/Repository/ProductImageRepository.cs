using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(ProductImage image)
        {
            _db.ProductImages.Update(image);
        }

        public override ProductImage Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<ProductImage> query = tracked ? dbSet : dbSet.AsNoTracking();
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
