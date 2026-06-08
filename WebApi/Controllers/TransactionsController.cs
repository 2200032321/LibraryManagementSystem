using System.Security.Claims;
using LibraryManagementSystem.BLL.Services;
using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        private static TransactionReadDto MapToDto(BookTransaction t)
        {
            return new TransactionReadDto
            {
                Id = t.Id,
                BookId = t.BookId,
                UserId = t.UserId,
               

                IssueDate = t.IssueDate,
                DueDate = t.DueDate,
               

               

                Status = t.Status.ToString()
            };
        }

        // ISSUE BOOK
        [HttpPost("issue")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Issue(
            [FromQuery] int bookId,
            [FromQuery] int userId)
        {
            var result =
                await _service.IssueBookAsync(bookId, userId, CurrentUserId);

            if (result == null)
                return BadRequest(
                    ApiResponse<string>.Fail("Book not available"));

            return Ok(
                ApiResponse<TransactionReadDto>.Ok(
                    MapToDto(result),
                    "Book issued"));
        }

        // RETURN BOOK
        [HttpPost("return/{transactionId}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Return(int transactionId)
        {
            var result =
                await _service.ReturnBookAsync(transactionId, CurrentUserId);

            if (result == null)
                return BadRequest(
                    ApiResponse<string>.Fail("Invalid transaction"));

            return Ok(
                ApiResponse<TransactionReadDto>.Ok(
                    MapToDto(result),
                    "Book returned"));
        }

        // COLLECT FINE
        [HttpPost("collect-fine/{transactionId}")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> CollectFine(int transactionId)
        {
            var result = await _service.CollectFineAsync(transactionId);

            if (!result)
                return NotFound();

            return Ok(
                ApiResponse<string>.Ok(
                    "",
                    "Fine collected"));
        }

        // USER HISTORY
        [HttpGet("history")]
        public async Task<IActionResult> MyHistory()
        {
            var data =
                await _service.GetUserHistoryAsync(CurrentUserId);

            var dto =
                data.Select(MapToDto);

            return Ok(
                ApiResponse<IEnumerable<TransactionReadDto>>
                    .Ok(dto));
        }

        // ALL TRANSACTIONS
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> All()
        {
            var data =
                await _service.GetAllTransactionsAsync();

            var dto =
                data.Select(MapToDto);

            return Ok(
                ApiResponse<IEnumerable<TransactionReadDto>>
                    .Ok(dto));
        }

        // OVERDUE BOOKS
        [HttpGet("overdue")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> Overdue()
        {
            var data =
                await _service.GetOverdueAsync();

            var dto =
                data.Select(MapToDto);

            return Ok(
                ApiResponse<IEnumerable<TransactionReadDto>>
                    .Ok(dto));
        }
    }
}