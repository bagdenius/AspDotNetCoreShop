using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId"), ValidateNever]
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "The value should be between 1 and 1000")]
        public int Count { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId"), ValidateNever]
        public User User { get; set; }
    }
}
