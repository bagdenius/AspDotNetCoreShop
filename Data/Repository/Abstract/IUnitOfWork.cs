namespace Data.Repository.Abstract
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        ICartItemRepository CartItem { get; }
        IUserRepository User { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }
        void Save();
    }
}
