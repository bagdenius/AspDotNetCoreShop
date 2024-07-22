using Models;

namespace Data.Repository.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        new void Update(Product product);
    }
}
