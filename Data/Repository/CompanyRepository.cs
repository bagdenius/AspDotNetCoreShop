using Data.Database;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public new void Update(Company company)
        {
            _db.Companies.Update(company);
        }

        public override Company Get(string id, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Company> query = tracked ? dbSet : dbSet.AsNoTracking();
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
