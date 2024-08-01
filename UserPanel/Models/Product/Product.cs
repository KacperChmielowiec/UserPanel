using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.Product
{
    public class Product
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        public bool InStock { get; set; } = true;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Url { get; set; }

    }
}
