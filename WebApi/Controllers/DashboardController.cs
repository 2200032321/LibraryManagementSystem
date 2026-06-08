using LibraryManagementSystem.BLL.Services;
using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.Context;
using LibraryManagementSystem.DOL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Librarian")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
       

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var data = await _service.GetStatsAsync();

            return Ok(
                ApiResponse<DashboardStatsDto>.Ok(
                    data,
                    "Dashboard statistics"
                )
            );
        }

       
    }
}