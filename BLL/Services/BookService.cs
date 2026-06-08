using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.UnitOfWork;
using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _uow;

        public BookService(IUnitOfWork uow) => _uow = uow;

       
        private readonly IAuditService _audit;

        public BookService(IUnitOfWork uow, IAuditService audit)
        {
            _uow = uow;
            _audit = audit;
        }

        // -------------------- GET ALL --------------------
        public async Task<IEnumerable<BookReadDto>> GetAllBooksAsync()
        {
            return await _uow.Books.Query()
                .Include(b => b.Category)
                .Select(b => new BookReadDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    Publisher = b.Publisher,
                    CategoryName = b.Category!.Name,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    Description = b.Description,
                    IsEBook = b.IsEBook,
                    PublishYear = b.PublishYear
                })
                .ToListAsync();
        }

        // -------------------- GET BY ID --------------------
        public async Task<BookReadDto?> GetBookByIdAsync(int id)
        {
            var b = await _uow.Books.Query()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b == null) return null;

            return new BookReadDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                ISBN = b.ISBN,
                Publisher = b.Publisher,
                CategoryName = b.Category!.Name,
                TotalCopies = b.TotalCopies,
                AvailableCopies = b.AvailableCopies,
                Description = b.Description,
                IsEBook = b.IsEBook,
                PublishYear = b.PublishYear
            };
        }

        // -------------------- SEARCH --------------------
        public async Task<IEnumerable<BookReadDto>> SearchBooksAsync(string keyword)
        {
            return await _uow.Books.Query()
                .Include(b => b.Category)
                .Where(b => b.Title.Contains(keyword) ||
                            b.Author.Contains(keyword) ||
                            (b.ISBN != null && b.ISBN.Contains(keyword)))
                .Select(b => new BookReadDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    Publisher = b.Publisher,
                    CategoryName = b.Category!.Name,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    Description = b.Description,
                    IsEBook = b.IsEBook,
                    PublishYear = b.PublishYear
                })
                .ToListAsync();
        }

        // -------------------- EBOOKS --------------------
        public async Task<IEnumerable<BookReadDto>> GetEBooksAsync()
        {
            return await _uow.Books.Query()
                .Include(b => b.Category)
                .Where(b => b.IsEBook)
                .Select(b => new BookReadDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ISBN = b.ISBN,
                    Publisher = b.Publisher,
                    CategoryName = b.Category!.Name,
                    TotalCopies = b.TotalCopies,
                    AvailableCopies = b.AvailableCopies,
                    Description = b.Description,
                    IsEBook = b.IsEBook,
                    PublishYear = b.PublishYear
                })
                .ToListAsync();
        }

        // -------------------- CREATE --------------------
        public async Task<BookReadDto> AddBookAsync(BookCreateDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                Publisher = dto.Publisher,
                CategoryId = dto.CategoryId,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies,
                Description = dto.Description,
                CoverImageUrl = dto.CoverImageUrl,
                IsEBook = dto.IsEBook,
                EBookFileUrl = dto.EBookFileUrl,
                PublishYear = dto.PublishYear
            };

            await _uow.Books.AddAsync(book);
            await _uow.CompleteAsync();

            await _audit.LogAsync(
                userId: null,
                action: "BOOK_CREATED",
                entityName: "Book",
                entityId: book.Id,
                description: $"Book '{book.Title}' created"
            );

            var category = await _uow.Categories.GetByIdAsync(book.CategoryId);

            return new BookReadDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Publisher = book.Publisher,
                CategoryName = category?.Name ?? "",
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                Description = book.Description,
                IsEBook = book.IsEBook,
                PublishYear = book.PublishYear
            };
        }

        // -------------------- UPDATE --------------------
        public async Task<BookReadDto?> UpdateBookAsync(int id, BookCreateDto dto)
        {
            var existing = await _uow.Books.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Title = dto.Title;
            existing.Author = dto.Author;
            existing.ISBN = dto.ISBN;
            existing.Publisher = dto.Publisher;
            existing.CategoryId = dto.CategoryId;
            existing.TotalCopies = dto.TotalCopies;
            existing.Description = dto.Description;
            existing.CoverImageUrl = dto.CoverImageUrl;
            existing.IsEBook = dto.IsEBook;
            existing.EBookFileUrl = dto.EBookFileUrl;
            existing.PublishYear = dto.PublishYear;

            _uow.Books.Update(existing);
            await _uow.CompleteAsync();

            await _audit.LogAsync(
                userId: null,
                action: "BOOK_UPDATED",
                entityName: "Book",
                entityId: existing.Id,
                description: $"Book '{existing.Title}' updated"
            );

            var category = await _uow.Categories.GetByIdAsync(existing.CategoryId);

            return new BookReadDto
            {
                Id = existing.Id,
                Title = existing.Title,
                Author = existing.Author,
                ISBN = existing.ISBN,
                Publisher = existing.Publisher,
                CategoryName = category?.Name ?? "",
                TotalCopies = existing.TotalCopies,
                AvailableCopies = existing.AvailableCopies,
                Description = existing.Description,
                IsEBook = existing.IsEBook,
                PublishYear = existing.PublishYear
            };
        }

        // -------------------- DELETE --------------------
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _uow.Books.GetByIdAsync(id);
            if (book == null) return false;

            _uow.Books.Remove(book);
            await _uow.CompleteAsync();

            await _audit.LogAsync(
                userId: null,
                action: "BOOK_DELETED",
                entityName: "Book",
                entityId: book.Id,
                description: $"Book '{book.Title}' deleted"
            );

            return true;
        }

        // -------------------- CATEGORIES --------------------
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _uow.Categories.GetAllAsync();
        }

        //Task<IEnumerable<Book>> IBookService.GetAllBooksAsync()
        //{
        //    //throw new NotImplementedException();
        //}

       
    }
}