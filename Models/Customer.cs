using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public DateTime RegisteredDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Calculated property
        public string FullName => $"{FirstName} {LastName}";
    }
}