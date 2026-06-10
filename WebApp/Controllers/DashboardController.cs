using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient()
        {
            var token = GetToken();

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7150/");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        // =========================
        // ENTRY POINT
        // =========================
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var role = GetRole();

            if (role == "Student")
                return await StudentDashboard();

            return await AdminLibrarianDashboard();
        }

        // =========================
        // ADMIN + LIBRARIAN DASHBOARD
        // =========================
        private async Task<IActionResult> AdminLibrarianDashboard()
        {
            var client = GetClient();

            var model = new DashboardViewModel();

            model.TotalBooks = await SafeCount(client, "api/books");
            model.TotalUsers = await SafeCount(client, "api/users");
            model.TotalTransactions = await SafeCount(client, "api/transactions/all");
            model.OverdueBooks = await SafeCount(client, "api/transactions/overdue");
            model.TotalReservations =
                await SafeCount(
                    client,
                    "api/reservations/all");

            return View("AdminDashboard", model);
        }

        // =========================
        // STUDENT DASHBOARD (SAFE)
        // =========================
        private async Task<IActionResult> StudentDashboard()
        {
            var client = GetClient();

            var model = new DashboardViewModel
            {
                TotalBooks = await SafeCount(client, "api/books")
            };

            return View("StudentDashboard", model);
        }

        // =========================
        // SAFE API CALL
        // =========================
        private async Task<int> SafeCount(HttpClient client, string url)
        {
            try
            {
                var json = await client.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                    return root.GetArrayLength();

                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty("data", out var data) &&
                    data.ValueKind == JsonValueKind.Array)
                {
                    return data.GetArrayLength();
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}