using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.DOL.Entities
{
    public class BookTransaction
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal FineAmount { get; set; }

        public bool IsFinePaid { get; set; }

        public TransactionStatus Status { get; set; } = TransactionStatus.Issued;

        // Librarian who handled it
        [ForeignKey(nameof(HandledByUserId))]
        public int? HandledByUserId { get; set; }
    }

   
}