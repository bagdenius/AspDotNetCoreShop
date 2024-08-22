using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class OrderItem
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string OrderId { get; set; }
        [ForeignKey(nameof(OrderId)), ValidateNever]
        public Order Order { get; set; }

        [Required]
        public string ProductId { get; set; }
        [ForeignKey(nameof(ProductId)), ValidateNever]
        public Product Product { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
    }
}
