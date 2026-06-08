using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.DOL.Entities;
using LibraryManagementSystem.DAL.Context;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserReadDto>> GetAllAsync()
    {
        return await _context.Users
            .Select(u => new UserReadDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                IsActive = u.IsActive,
                Role = u.Role.ToString()
            })
            .ToListAsync();
    }

    public async Task<UserReadDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null) return null;

        return new UserReadDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            IsActive = user.IsActive,
            Role = user.Role.ToString()
        };
    }

    public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.Address = dto.Address;
        user.Role = dto.Role;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.IsActive = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}