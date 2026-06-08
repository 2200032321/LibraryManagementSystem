using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DOL.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}