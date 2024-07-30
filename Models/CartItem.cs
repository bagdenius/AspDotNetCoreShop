using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CartItem
    {
        [Key]
        public string Id { get; set; } = Guid.Empty.ToString();

        public string ProductId { get; set; } = Guid.Empty.ToString();

        [ForeignKey("ProductId"), ValidateNever]
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "The value should be between 1 and 1000")]
        public int Count { get; set; }

        public string UserId { get; set; } = Guid.Empty.ToString();

        [ForeignKey("UserId"), ValidateNever]
        public User User { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
