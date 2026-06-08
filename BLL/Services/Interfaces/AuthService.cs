using LibraryManagementSystem.BLL.Helpers;
using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.UnitOfWork;
using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;

namespace LibraryManagementSystem.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUnitOfWork uow, JwtHelper jwtHelper)
        {
            _uow = uow;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = (await _uow.Users.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null || !user.IsActive) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            return _jwtHelper.GenerateToken(user);
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var exists = (await _uow.Users.FindAsync(u => u.Email == dto.Email)).Any();
            if (exists) return null;

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = dto.Role,
                IsActive = true
            };

            await _uow.Users.AddAsync(user);
            await _uow.CompleteAsync();

            return _jwtHelper.GenerateToken(user);
        }
    }
}