using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(OrderItem orderDetail)
        {
            _db.OrderItems.Update(orderDetail);
        }

        public override OrderItem Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<OrderItem> query = tracked ? dbSet : dbSet.AsNoTracking();
            query = query.Where(od => od.Id == id);
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
