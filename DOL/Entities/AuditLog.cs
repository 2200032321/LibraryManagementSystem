using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DOL.Entities
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }   // who performed action (Admin/Librarian)

        [Required]
        public string Action { get; set; } = string.Empty;
        // e.g. "BOOK_ISSUED", "BOOK_RETURNED"

        [Required]
        public string EntityName { get; set; } = string.Empty;
        // e.g. "Book", "Reservation"

        public int? EntityId { get; set; }
        // affected record id

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}