using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Company
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }
}
