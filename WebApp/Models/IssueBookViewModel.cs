namespace WebApp.Models
{
    public class IssueBookViewModel
    {
        public int BookId { get; set; }

        public int UserId { get; set; }

        public List<BookDropdown> Books { get; set; } = new();

        public List<UserDropdown> Users { get; set; } = new();
    }

    public class BookDropdown
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
    }

    public class UserDropdown
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Role { get; set; } = "";
    }
}