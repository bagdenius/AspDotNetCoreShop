using Data.Database;
using Data.Repository.Abstract;

namespace Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }

        public UnitOfWork(ApplicationDbContext db,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ICompanyRepository companyRepository)
        {
            _db = db;
            //Category = new CategoryRepository(_db);
            //Product = new ProductRepository(_db);
            //Company = new CompanyRepository(_db);
            Category = categoryRepository;
            Product = productRepository;
            Company = companyRepository;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
