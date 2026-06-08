using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DOL.Entities;

namespace LibraryManagementSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Book> Books { get; }
        IRepository<Category> Categories { get; }
        IRepository<BookTransaction> Transactions { get; }
        IRepository<Reservation> Reservations { get; }
        IRepository<Notification> Notifications { get; }
        Task<int> CompleteAsync();
    }
}