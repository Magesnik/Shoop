using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int ViewCount { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}