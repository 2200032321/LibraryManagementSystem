using System.Security.Claims;
using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DOL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(
            INotificationService service)
        {
            _service = service;
        }

        private int CurrentUserId =>
            int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> MyNotifications()
        {
            return Ok(
                ApiResponse<IEnumerable<NotificationReadDto>>
                .Ok(
                    await _service
                    .GetUserNotificationsAsync(CurrentUserId)));
        }

        [HttpPut("read/{id}")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var result =
                await _service.MarkAsReadAsync(id);

            return result
                ? Ok(ApiResponse<string>
                    .Ok("", "Marked as read"))
                : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _service.DeleteAsync(id);

            return result
                ? Ok(ApiResponse<string>
                    .Ok("", "Deleted"))
                : NotFound();
        }
    }
}