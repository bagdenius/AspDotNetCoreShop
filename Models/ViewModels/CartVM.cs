namespace Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<CartItem> Items { get; set; }
        public Order Order { get; set; }
    }
}
