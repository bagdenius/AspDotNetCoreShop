using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(Order order)
        {
            _db.Orders.Update(order);
        }

        public override Order Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Order> query = tracked ? dbSet : dbSet.AsNoTracking();
            query = query.Where(o => o.Id == id);
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

        public void UpdateStatus(string id, string status, string? paymentStatus = null)
        {
            Order order = _db.Orders.Find(id);
            if (order != null)
            {
                order.Status = status;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    order.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(string id, string sessionId, string paymentIntentId)
        {
            Order order = _db.Orders.Find(id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                order.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                order.PaymentIntentId = paymentIntentId;
                order.PaymentDate = DateTime.Now;
            }
        }
    }
}
