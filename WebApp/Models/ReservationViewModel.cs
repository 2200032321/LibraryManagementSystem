namespace WebApp.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }

        public DateTime ReservedAt { get; set; }

        public string Status { get; set; } = "";
    }
}