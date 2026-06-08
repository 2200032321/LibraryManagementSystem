using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.UnitOfWork;
using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;

        public ReportService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<BookReportDto>> GetBooksReportAsync()
        {
            return await _uow.Books.Query()
                .Include(x => x.Category)
                .Select(x => new BookReportDto
                {
                    BookId = x.Id,
                    Title = x.Title,
                    Category = x.Category!.Name,
                    TotalCopies = x.TotalCopies,
                    AvailableCopies = x.AvailableCopies
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<IssuedBookReportDto>> GetIssuedBooksAsync()
        {
            return await _uow.Transactions.Query()
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x => x.Status == TransactionStatus.Issued)
                .Select(x => new IssuedBookReportDto
                {
                    BookTitle = x.Book!.Title,
                    UserName = x.User!.FullName,
                    IssueDate = x.IssueDate,
                    DueDate = x.DueDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<OverdueReportDto>> GetOverdueBooksAsync()
        {
            var transactions = await _uow.Transactions.Query()
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x =>
                    x.Status == TransactionStatus.Issued &&
                    x.DueDate < DateTime.UtcNow)
                .ToListAsync();

            return transactions.Select(x => new OverdueReportDto
            {
                BookTitle = x.Book!.Title,
                UserName = x.User!.FullName,
                DaysOverdue = (DateTime.UtcNow - x.DueDate).Days,
                FineAmount = x.FineAmount
            });
        }

        public async Task<IEnumerable<FineReportDto>> GetFineReportAsync()
        {
            return await _uow.Transactions.Query()
                .Include(x => x.Book)
                .Include(x => x.User)
                .Where(x => x.FineAmount > 0)
                .Select(x => new FineReportDto
                {
                    UserName = x.User!.FullName,
                    BookTitle = x.Book!.Title,
                    FineAmount = x.FineAmount,
                    Paid = x.IsFinePaid
                })
                .ToListAsync();
        }
    }
}