using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.UnitOfWork;
using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;

        public NotificationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<NotificationReadDto>>
            GetUserNotificationsAsync(int userId)
        {
            return await _uow.Notifications.Query()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new NotificationReadDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            var notification =
                await _uow.Notifications.GetByIdAsync(notificationId);

            if (notification == null)
                return false;

            notification.IsRead = true;

            _uow.Notifications.Update(notification);

            await _uow.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            var notification =
                await _uow.Notifications.GetByIdAsync(notificationId);

            if (notification == null)
                return false;

            _uow.Notifications.Remove(notification);

            await _uow.CompleteAsync();

            return true;
        }

        public async Task CreateAsync(
            int userId,
            string title,
            string message)
        {
            await _uow.Notifications.AddAsync(
                new Notification
                {
                    UserId = userId,
                    Title = title,
                    Message = message
                });

            await _uow.CompleteAsync();
        }
    }
}