using Models;

namespace Data.Repository.Abstract
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        new void Update(OrderItem orderDetail);
    }
}
