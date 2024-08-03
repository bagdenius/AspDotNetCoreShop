using Models;

namespace Data.Repository.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        new void Update(Order order);
        void UpdateStatus(string id, string status, string? paymentStatus = null);
        void UpdateStripePaymentId(string id, string sessionId, string paymentIntentId);
    }
}
