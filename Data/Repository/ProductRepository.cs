using Data.Database;
using Data.Repository.Abstract;
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
            _db.Products.Update(product);
        }
    }
}
