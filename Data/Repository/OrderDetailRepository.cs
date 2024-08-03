using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(OrderDetail orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
        }

        public override OrderDetail Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<OrderDetail> query = tracked ? dbSet : dbSet.AsNoTracking();
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
