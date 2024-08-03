using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(Product product)
        {
            Product updatedProduct = Get(product.Id, tracked: true);
            if (updatedProduct != null)
            {
                updatedProduct.Title = product.Title;
                updatedProduct.Author = product.Author;
                updatedProduct.ISBN = product.ISBN;
                updatedProduct.Description = product.Description;
                updatedProduct.ListPrice = product.ListPrice;
                updatedProduct.Price = product.Price;
                updatedProduct.Price50 = product.Price50;
                updatedProduct.Price100 = product.Price100;
                updatedProduct.CategoryId = product.CategoryId;
                if (product.ImageUrl != null)
                {
                    updatedProduct.ImageUrl = product.ImageUrl;
                }
            }
        }

        public override Product Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Product> query = tracked ? dbSet : dbSet.AsNoTracking();
            query = query.Where(p => p.Id == id);
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
