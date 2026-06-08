namespace LibraryManagementSystem.DOL.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public string? Publisher { get; set; }

        public int CategoryId { get; set; }

        public int TotalCopies { get; set; }
        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        public bool IsEBook { get; set; }
        public string? EBookFileUrl { get; set; }

        public int PublishYear { get; set; }
    }
}