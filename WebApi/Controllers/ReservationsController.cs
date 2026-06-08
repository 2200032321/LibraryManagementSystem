using System.Security.Claims;
using LibraryManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpPost]
        public async Task<IActionResult> Reserve(int bookId)
        {
            var result = await _service.ReserveBookAsync(CurrentUserId, bookId);

            if (result == null)
                return NotFound("Book not found");

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyReservations()
        {
            return Ok(await _service.GetMyReservationsAsync(CurrentUserId));
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> AllReservations()
        {
            return Ok(await _service.GetAllReservationsAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelReservationAsync(id);

            return result ? Ok("Reservation cancelled")
                          : NotFound();
        }
    }
}