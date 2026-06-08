using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DAL.UnitOfWork;
using LibraryManagementSystem.DOL.DTOs;
using LibraryManagementSystem.DOL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.BLL.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _uow;

        public ReservationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ReservationReadDto?> ReserveBookAsync(int userId, int bookId)
        {
            var book = await _uow.Books.GetByIdAsync(bookId);

            if (book == null)
                return null;

            if (book.AvailableCopies > 0)
                throw new Exception("Book is available. Issue directly.");

            var existing = await _uow.Reservations.Query()
                .FirstOrDefaultAsync(r =>
                    r.BookId == bookId &&
                    r.UserId == userId &&
                    r.Status == ReservationStatus.Pending);

            if (existing != null)
                throw new Exception("You already reserved this book.");

            var reservation = new Reservation
            {
                BookId = bookId,
                UserId = userId,
                ReservationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(3),
                Status = ReservationStatus.Pending
            };

            await _uow.Reservations.AddAsync(reservation);
            await _uow.CompleteAsync();

            return new ReservationReadDto
            {
                Id = reservation.Id,
                BookId = reservation.BookId,
                BookTitle = book.Title,
                UserId = reservation.UserId,
                ReservationDate = reservation.ReservationDate,
                ExpiryDate = reservation.ExpiryDate,
                Status = reservation.Status.ToString()
            };
        }

        public async Task<IEnumerable<ReservationReadDto>> GetMyReservationsAsync(int userId)
        {
            return await _uow.Reservations.Query()
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .Select(r => new ReservationReadDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    BookTitle = r.Book!.Title,
                    UserId = r.UserId,
                    ReservationDate = r.ReservationDate,
                    ExpiryDate = r.ExpiryDate,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReservationReadDto>> GetAllReservationsAsync()
        {
            return await _uow.Reservations.Query()
                .Include(r => r.Book)
                .Select(r => new ReservationReadDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    BookTitle = r.Book!.Title,
                    UserId = r.UserId,
                    ReservationDate = r.ReservationDate,
                    ExpiryDate = r.ExpiryDate,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _uow.Reservations.GetByIdAsync(reservationId);

            if (reservation == null)
                return false;

            reservation.Status = ReservationStatus.Cancelled;

            _uow.Reservations.Update(reservation);

            await _uow.CompleteAsync();

            return true;
        }
    }
}