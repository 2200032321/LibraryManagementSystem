using LibraryManagementSystem.DOL.DTOs;

namespace LibraryManagementSystem.BLL.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<BookReportDto>> GetBooksReportAsync();

        Task<IEnumerable<IssuedBookReportDto>> GetIssuedBooksAsync();

        Task<IEnumerable<OverdueReportDto>> GetOverdueBooksAsync();

        Task<IEnumerable<FineReportDto>> GetFineReportAsync();
    }
}