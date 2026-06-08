using LibraryManagementSystem.DOL.DTOs;

namespace LibraryManagementSystem.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationReadDto>> GetUserNotificationsAsync(int userId);

        Task<bool> MarkAsReadAsync(int notificationId);

        Task<bool> DeleteAsync(int notificationId);

        Task CreateAsync(int userId, string title, string message);
    }
}