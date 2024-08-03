using Models;

namespace Data.Repository.Abstract
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        new void Update(OrderDetail orderDetail);
    }
}
