using Models;

namespace Data.Repository.Abstract
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        new void Update(ProductImage image);
    }
}
