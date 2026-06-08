using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;

namespace LibraryManagementSystem.BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookReadDto>> GetAllBooksAsync();
        Task<BookReadDto?> GetBookByIdAsync(int id);
        Task<IEnumerable<BookReadDto>> SearchBooksAsync(string keyword);
        Task<IEnumerable<BookReadDto>> GetEBooksAsync();
        Task<BookReadDto> AddBookAsync(BookCreateDto dto);
        Task<BookReadDto?> UpdateBookAsync(int id, BookCreateDto dto);
        Task<bool> DeleteBookAsync(int id);
        
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}