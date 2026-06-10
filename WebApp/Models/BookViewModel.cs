namespace WebApp.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        // NEW FIELDS
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? EBookFileUrl { get; set; }
        public bool IsEBook { get; set; }
    }
}