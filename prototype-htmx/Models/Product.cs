using System.ComponentModel.DataAnnotations;

namespace htmx.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public double Price { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public string? PreviewImage { get; set; }
    }
}