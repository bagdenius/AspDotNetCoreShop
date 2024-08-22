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
        public ICartItemRepository CartItem { get; private set; }
        public IUserRepository User { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IOrderItemRepository OrderItem { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }

        public UnitOfWork(ApplicationDbContext db,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ICompanyRepository companyRepository,
            ICartItemRepository cartItem,
            IUserRepository user,
            IOrderRepository order,
            IOrderItemRepository orderItem,
            IProductImageRepository productImage)
        {
            _db = db;
            Category = categoryRepository;
            Product = productRepository;
            Company = companyRepository;
            CartItem = cartItem;
            User = user;
            Order = order;
            OrderItem = orderItem;
            ProductImage = productImage;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
