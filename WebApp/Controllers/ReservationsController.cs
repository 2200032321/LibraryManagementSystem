using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ReservationsController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservationsController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient()
        {
            var client =
                _httpClientFactory.CreateClient();

            client.BaseAddress =
                new Uri("https://localhost:7150/");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    GetToken());

            return client;
        }

        // =========================
        // MY RESERVATIONS
        // =========================
        public async Task<IActionResult> MyReservations()
        {
            if (GetRole() != "Student")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            var client = GetClient();

            var response =
                await client.GetAsync(
                    "api/reservations/my");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<ReservationViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            List<ReservationViewModel> reservations =
                new();

            try
            {
                using var doc =
                    JsonDocument.Parse(json);

                if (doc.RootElement.ValueKind ==
                    JsonValueKind.Array)
                {
                    reservations =
                        JsonSerializer.Deserialize<
                            List<ReservationViewModel>>
                        (
                            json,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        ) ?? new();
                }
                else if (
                    doc.RootElement.TryGetProperty(
                        "data",
                        out var data))
                {
                    reservations =
                        JsonSerializer.Deserialize<
                            List<ReservationViewModel>>
                        (
                            data.GetRawText(),
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        ) ?? new();
                }
            }
            catch
            {
                reservations = new();
            }

            return View(reservations);
        }

        // =========================
        // RESERVE BOOK
        // =========================
        [HttpPost]
        public async Task<IActionResult> Reserve(int bookId)
        {
            if (GetRole() != "Student")
            {
                TempData["Error"] =
                    "Only students can reserve books.";

                return RedirectToAction(
                    "Index",
                    "Books");
            }

            var client = GetClient();

            var response =
                await client.PostAsync(
                    $"api/reservations?bookId={bookId}",
                    null);

            if (!response.IsSuccessStatusCode)
            {
                var error =
                    await response.Content
                        .ReadAsStringAsync();

                TempData["Error"] =
                    $"Unable to reserve book. {error}";
            }
            else
            {
                TempData["Success"] =
                    "Book reserved successfully.";
            }

            return RedirectToAction(
                "Index",
                "Books");
        }

        // =========================
        // CANCEL RESERVATION
        // =========================
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            if (GetRole() != "Student")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            var client = GetClient();

            var response =
                await client.DeleteAsync(
                    $"api/reservations/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Reservation cancelled.";
            }
            else
            {
                TempData["Error"] =
                    "Unable to cancel reservation.";
            }

            return RedirectToAction(
                nameof(MyReservations));
        }
    }
}