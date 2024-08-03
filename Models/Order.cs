using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.Empty.ToString();

        // Order info
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public DateTime Date { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateOnly PaymentDueDate { get; set; }
        public double Total { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }

        // Shipping info        
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime ShippingDate { get; set; }

        // Customer user info
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId)), ValidateNever]
        public User User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PostalCode { get; set; }
    }
}
