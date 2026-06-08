using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DOL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Librarian")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpGet("books")]
        public async Task<IActionResult> Books()
            => Ok(ApiResponse<object>.Ok(
                await _service.GetBooksReportAsync()));

        [HttpGet("issued-books")]
        public async Task<IActionResult> IssuedBooks()
            => Ok(ApiResponse<object>.Ok(
                await _service.GetIssuedBooksAsync()));

        [HttpGet("overdue-books")]
        public async Task<IActionResult> OverdueBooks()
            => Ok(ApiResponse<object>.Ok(
                await _service.GetOverdueBooksAsync()));

        [HttpGet("fines")]
        public async Task<IActionResult> Fines()
            => Ok(ApiResponse<object>.Ok(
                await _service.GetFineReportAsync()));
    }
}