using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportsController(
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

        public async Task<IActionResult> Index()
        {
            if (GetRole() == "Student")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            var client = GetClient();

            var model = new ReportViewModel();

            model.TotalBooks =
                await SafeCount(client, "api/books");

            model.TotalUsers =
                await SafeCount(client, "api/users");

            model.TotalTransactions =
                await SafeCount(client, "api/transactions/all");

            model.OverdueBooks =
                await SafeCount(client, "api/transactions/overdue");

            model.TotalReservations =
                await SafeCount(client, "api/reservations/all");

            return View(model);
        }

        private async Task<int> SafeCount(
            HttpClient client,
            string url)
        {
            try
            {
                var json =
                    await client.GetStringAsync(url);

                using var doc =
                    JsonDocument.Parse(json);

                var root =
                    doc.RootElement;

                if (root.ValueKind ==
                    JsonValueKind.Array)
                {
                    return root.GetArrayLength();
                }

                if (root.TryGetProperty(
                    "data",
                    out var data))
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