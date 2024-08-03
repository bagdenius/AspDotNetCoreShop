using Models;

namespace Data.Repository.Abstract
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        new void Update(CartItem shoppingCart);
    }
}
