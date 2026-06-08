using LibraryManagementSystem.DOL.DTOs;

namespace LibraryManagementSystem.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
    }
}