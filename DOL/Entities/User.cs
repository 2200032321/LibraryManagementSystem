using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DOL.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<BookTransaction> Transactions { get; set; } = new List<BookTransaction>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

    public enum UserRole
    {
        Admin = 1,
        Librarian = 2,
        Student = 3
    }
}