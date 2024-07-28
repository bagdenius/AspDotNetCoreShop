using Data.Database;
using Data.Repository.Abstract;
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
    }
}
