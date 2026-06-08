using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.UnitOfWork;
using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.BLL.Services
{
    //public interface IDashboardService
    //{
    //    Task<DashboardStatsDto> GetStatsAsync();
    //}

    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _uow;

        public DashboardService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<DashboardStatsDto> GetStatisticsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var books = await _uow.Books.Query()
                .Include(b => b.Category)
                .ToListAsync();

            var transactions = await _uow.Transactions.Query()
                .ToListAsync();

            var members = await _uow.Users.Query()
                .Where(u => u.Role == UserRole.Student)
                .ToListAsync();

            var reservations = await _uow.Reservations.Query()
                .ToListAsync();

            return new DashboardStatsDto
            {
                TotalBooks = books.Sum(x => x.TotalCopies),

                AvailableBooks = books.Sum(x => x.AvailableCopies),

                TotalMembers = members.Count,

                ActiveMembers = members.Count(x => x.IsActive),

                IssuedBooks = transactions.Count(x =>
                    x.Status == TransactionStatus.Issued),

                OverdueBooks = transactions.Count(x =>
                    x.Status == TransactionStatus.Issued &&
                    x.DueDate < DateTime.UtcNow),

                EBooks = books.Count(x => x.IsEBook),

                TotalFineCollected = transactions
                    .Where(x => x.IsFinePaid)
                    .Sum(x => x.FineAmount),

                TotalReservations = reservations.Count,

                BooksByCategory = books
                    .GroupBy(x => x.Category!.Name)
                    .Select(g => new CategoryStatDto
                    {
                        CategoryName = g.Key,
                        Count = g.Count()
                    })
                    .ToList(),

                MonthlyIssues = transactions
                    .GroupBy(x => x.IssueDate.ToString("MMM yyyy"))
                    .Select(g => new MonthlyStatDto
                    {
                        Month = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Month)
                    .ToList()
            };
        }
    }
}