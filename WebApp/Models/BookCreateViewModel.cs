namespace WebApp.Models
{
    public class BookCreateViewModel
    {
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string? ISBN { get; set; }
        public string? Publisher { get; set; }

        public int CategoryId { get; set; }

        public int TotalCopies { get; set; }

        public string? Description { get; set; }

        public IFormFile? CoverImageUrl{ get; set; }

        public bool IsEBook { get; set; }

        public string? EBookFileUrl { get; set; }

        public int PublishYear { get; set; }
    }
}