using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ProductImage
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Url { get; set; }


        public string ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
