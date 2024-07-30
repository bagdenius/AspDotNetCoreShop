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
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext db,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ICompanyRepository companyRepository,
            IShoppingCartRepository shoppingCart,
            IUserRepository user)
        {
            _db = db;
            Category = categoryRepository;
            Product = productRepository;
            Company = companyRepository;
            ShoppingCart = shoppingCart;
            User = user;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
