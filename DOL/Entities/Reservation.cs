using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.DOL.Entities
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }

    public enum ReservationStatus
    {
        Pending = 1,
        Fulfilled = 2,
        Cancelled = 3,
        Expired = 4
    }
}