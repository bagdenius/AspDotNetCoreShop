using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _db;
        public CartItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(CartItem shoppingCart)
        {
            _db.CartItems.Update(shoppingCart);
        }

        public override CartItem Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<CartItem> query = tracked ? dbSet : dbSet.AsNoTracking();
            query = query.Where(ci => ci.Id == id);
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
