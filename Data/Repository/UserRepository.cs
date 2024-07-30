using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public override User Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<User> query = tracked ? dbSet : dbSet.AsNoTracking();
            query = query.Where(c => c.Id == id);
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
