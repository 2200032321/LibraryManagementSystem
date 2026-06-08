using LibraryManagementSystem.DOL.DTOs;
using System.Security.AccessControl;

namespace LibraryManagementSystem.BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
    }
}