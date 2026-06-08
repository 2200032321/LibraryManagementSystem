using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DOL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // -------------------- GET ALL --------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _bookService.GetAllBooksAsync());

        // -------------------- GET BY ID --------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return book == null ? NotFound() : Ok(book);
        }

        // -------------------- SEARCH --------------------
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
            => Ok(await _bookService.SearchBooksAsync(keyword ?? ""));

        // -------------------- EBOOKS --------------------
        [HttpGet("ebooks")]
        public async Task<IActionResult> GetEBooks()
            => Ok(await _bookService.GetEBooksAsync());

        // -------------------- CATEGORIES --------------------
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
            => Ok(await _bookService.GetCategoriesAsync());

        // -------------------- CREATE --------------------
        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Create([FromBody] BookCreateDto dto)
            => Ok(await _bookService.AddBookAsync(dto));

        // -------------------- UPDATE --------------------
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Update(int id, [FromBody] BookCreateDto dto)
        {
            var result = await _bookService.UpdateBookAsync(id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        // -------------------- DELETE --------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            return result ? Ok("Deleted") : NotFound();
        }
    }
}