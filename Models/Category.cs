using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; } = Guid.Empty.ToString();

        [Required, DisplayName("Category Name"), MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Display Order"), Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
