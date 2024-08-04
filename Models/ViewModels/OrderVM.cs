namespace Models.ViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
    }
}
