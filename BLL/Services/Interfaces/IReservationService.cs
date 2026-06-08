using LibraryManagementSystem.DOL.DTOs;

namespace LibraryManagementSystem.BLL.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationReadDto?> ReserveBookAsync(int userId, int bookId);

        Task<IEnumerable<ReservationReadDto>> GetMyReservationsAsync(int userId);

        Task<IEnumerable<ReservationReadDto>> GetAllReservationsAsync();

        Task<bool> CancelReservationAsync(int reservationId);
    }
}