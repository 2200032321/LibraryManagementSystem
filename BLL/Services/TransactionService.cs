using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.UnitOfWork;
using LibraryManagementSystem.DOL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.BLL.Services
{
    public interface ITransactionService
    {
        Task<BookTransaction?> IssueBookAsync(int bookId, int userId, int librarianId);
        Task<BookTransaction?> ReturnBookAsync(int transactionId, int librarianId);
        Task<bool> CollectFineAsync(int transactionId);
        Task<IEnumerable<BookTransaction>> GetUserHistoryAsync(int userId);
        Task<IEnumerable<BookTransaction>> GetAllTransactionsAsync();
        Task<IEnumerable<BookTransaction>> GetOverdueAsync();
    }

    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IAuditService _audit;

        private const decimal FinePerDay = 5.00m;
        private const int LoanPeriodDays = 14;

        public TransactionService(IUnitOfWork uow, IAuditService audit)
        {
            _uow = uow;
            _audit = audit;
        }

        // -------------------- ISSUE BOOK --------------------
        public async Task<BookTransaction?> IssueBookAsync(int bookId, int userId, int librarianId)
        {
            var book = await _uow.Books.GetByIdAsync(bookId);

            if (book == null || book.AvailableCopies <= 0)
                return null;

            book.AvailableCopies--;
            _uow.Books.Update(book);

            var transaction = new BookTransaction
            {
                BookId = bookId,
                UserId = userId,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(LoanPeriodDays),
                Status = TransactionStatus.Issued,
                HandledByUserId = librarianId
            };

            // ✅ FIX: Save transaction first
            await _uow.Transactions.AddAsync(transaction);

            await _uow.Notifications.AddAsync(
              new Notification
              {
                  UserId = userId,
                  Title = "Book Issued",
                  Message = $"Book '{book.Title}' issued successfully."
              });

            await _uow.CompleteAsync();

            // ✅ AUDIT LOG
            await _audit.LogAsync(
                userId: librarianId,
                action: "BOOK_ISSUED",
                entityName: "BookTransaction",
                entityId: transaction.Id,
                description: $"BookId {bookId} issued to UserId {userId}"
            );

            return transaction;
        }

        // -------------------- RETURN BOOK --------------------
        public async Task<BookTransaction?> ReturnBookAsync(int transactionId, int librarianId)
        {
            var transaction = await _uow.Transactions.GetByIdAsync(transactionId);

            if (transaction == null || transaction.Status == TransactionStatus.Returned)
                return null;

            transaction.ReturnDate = DateTime.UtcNow;
            transaction.Status = TransactionStatus.Returned;
            transaction.HandledByUserId = librarianId;

            // Calculate fine if overdue
            if (transaction.ReturnDate > transaction.DueDate)
            {
                var overdueDays = (transaction.ReturnDate.Value - transaction.DueDate).Days;
                transaction.FineAmount = overdueDays * FinePerDay;
            }

            var book = await _uow.Books.GetByIdAsync(transaction.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
                _uow.Books.Update(book);
            }

            _uow.Transactions.Update(transaction);

            await _uow.Notifications.AddAsync(
                new Notification
                {
                    UserId = transaction.UserId,
                    Title = "Book Returned",
                    Message = $"Book '{transaction.Book?.Title}' returned."
                });

            await _uow.CompleteAsync();

            // ✅ AUDIT LOG
            await _audit.LogAsync(
                userId: librarianId,
                action: "BOOK_RETURNED",
                entityName: "BookTransaction",
                entityId: transaction.Id,
                description: $"BookId {transaction.BookId} returned by UserId {transaction.UserId}"
            );

            return transaction;
        }

        // -------------------- COLLECT FINE --------------------
        public async Task<bool> CollectFineAsync(int transactionId)
        {
            var transaction = await _uow.Transactions.GetByIdAsync(transactionId);

            if (transaction == null)
                return false;

            transaction.IsFinePaid = true;

            _uow.Transactions.Update(transaction);
            await _uow.CompleteAsync();

            // ✅ AUDIT LOG
            await _audit.LogAsync(
                userId: null,
                action: "FINE_COLLECTED",
                entityName: "BookTransaction",
                entityId: transactionId,
                description: $"Fine collected for TransactionId {transactionId}"
            );

            return true;
        }

        // -------------------- USER HISTORY --------------------
        public async Task<IEnumerable<BookTransaction>> GetUserHistoryAsync(int userId)
            => await _uow.Transactions.Query()
                .Include(t => t.Book)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.IssueDate)
                .ToListAsync();

        // -------------------- ALL TRANSACTIONS --------------------
        public async Task<IEnumerable<BookTransaction>> GetAllTransactionsAsync()
            => await _uow.Transactions.Query()
                .Include(t => t.Book)
                .Include(t => t.User)
                .OrderByDescending(t => t.IssueDate)
                .ToListAsync();

        // -------------------- OVERDUE --------------------
        public async Task<IEnumerable<BookTransaction>> GetOverdueAsync()
            => await _uow.Transactions.Query()
                .Include(t => t.Book)
                .Include(t => t.User)
                .Where(t => t.Status == TransactionStatus.Issued &&
                            t.DueDate < DateTime.UtcNow)
                .ToListAsync();
    }
}