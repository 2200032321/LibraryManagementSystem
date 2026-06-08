using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.DOL.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Author { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? ISBN { get; set; }

        [MaxLength(100)]
        public string? Publisher { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public string? Description { get; set; }

        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        // E-Book support
        public bool IsEBook { get; set; }
        [MaxLength(500)]
        public string? EBookFileUrl { get; set; }

        public int PublishYear { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<BookTransaction> Transactions { get; set; } = new List<BookTransaction>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}