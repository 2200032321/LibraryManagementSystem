namespace LibraryManagementSystem.DOL.DTOs
{
    public class BookReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;

        public string? ISBN { get; set; }
        public string? Publisher { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public string? Description { get; set; }

        public bool IsEBook { get; set; }

        public int PublishYear { get; set; }
    }
}