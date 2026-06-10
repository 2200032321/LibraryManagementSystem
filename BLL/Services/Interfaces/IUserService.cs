public interface IUserService
{
    Task<List<UserReadDto>> GetAllAsync();
    Task<UserReadDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UserUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<List<UserReadDto>> GetAllUsersAsync();
    Task<bool> ActivateAsync(int id);
    Task<bool> DeactivateAsync(int id);
}