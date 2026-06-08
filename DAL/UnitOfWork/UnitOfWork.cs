using LibraryManagementSystem.DAL.Context;
using LibraryManagementSystem.DAL.Repositories;
using LibraryManagementSystem.DOL.Entities;

namespace LibraryManagementSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<User> Users { get; }
        public IRepository<Book> Books { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<BookTransaction> Transactions { get; }
        public IRepository<Reservation> Reservations { get; }
        public IRepository<Notification> Notifications { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new Repository<User>(context);
            Books = new Repository<Book>(context);
            Categories = new Repository<Category>(context);
            Transactions = new Repository<BookTransaction>(context);
            Reservations = new Repository<Reservation>(context);
            Notifications = new Repository<Notification>(context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}