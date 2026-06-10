using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TransactionsController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TransactionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient()
        {
            var token = GetToken();

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress =
                new Uri("https://localhost:7150/");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        // =========================
        // ALL TRANSACTIONS
        // =========================
        public async Task<IActionResult> Index()
        {
            var role = GetRole();

            if (role == "Student")
            {
                return RedirectToAction(nameof(MyBooks));
            }

            var client = GetClient();

            var response =
                await client.GetAsync("api/transactions/all");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<TransactionViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var data =
                doc.RootElement.GetProperty("data");

            var transactions =
                JsonSerializer.Deserialize<List<TransactionViewModel>>
                (
                    data.GetRawText(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(
                transactions ?? new List<TransactionViewModel>());
        }

        // =========================
        // STUDENT BOOK HISTORY
        // =========================
        [HttpGet]
        public async Task<IActionResult> MyBooks()
        {
            var role = GetRole();

            if (role != "Student")
            {
                return RedirectToAction(nameof(Index));
            }

            var client = GetClient();

            var response =
                await client.GetAsync("api/transactions/history");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<TransactionViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var data =
                doc.RootElement.GetProperty("data");

            var transactions =
                JsonSerializer.Deserialize<List<TransactionViewModel>>
                (
                    data.GetRawText(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(
                transactions ?? new List<TransactionViewModel>());
        }

        // =========================
        // ISSUE PAGE
        // =========================
        [HttpGet]
        public async Task<IActionResult> Issue()
        {
            var role = GetRole();

            if (role == "Student")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            var model =
                new IssueBookViewModel();

            await LoadIssueDropdowns(model);

            return View(model);
        }

        // =========================
        // ISSUE BOOK
        // =========================
        [HttpPost]
        public async Task<IActionResult> Issue(
            IssueBookViewModel model)
        {
            var role = GetRole();

            if (role == "Student")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            if (model.BookId == 0 ||
                model.UserId == 0)
            {
                ViewBag.Error =
                    "Please select Book and User";

                await LoadIssueDropdowns(model);

                return View(model);
            }

            var client = GetClient();

            var response =
                await client.PostAsync(
                    $"api/transactions/issue?bookId={model.BookId}&userId={model.UserId}",
                    null);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error =
                    "Failed to issue book";

                await LoadIssueDropdowns(model);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // LOAD DROPDOWNS
        // =========================
        private async Task LoadIssueDropdowns(IssueBookViewModel model)
        {
            var client = GetClient();

            // ================= BOOKS =================

            var booksResponse = await client.GetAsync("api/books");

            if (booksResponse.IsSuccessStatusCode)
            {
                var booksJson =
                    await booksResponse.Content.ReadAsStringAsync();

                JsonElement booksData;

                using (var doc = JsonDocument.Parse(booksJson))
                {
                    booksData = doc.RootElement.Clone();
                }

                if (booksData.ValueKind == JsonValueKind.Object &&
                    booksData.TryGetProperty("data", out var d))
                {
                    booksData = d;
                }

                model.Books =
                    JsonSerializer.Deserialize<List<BookDropdown>>
                    (
                        booksData.GetRawText(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    ) ?? new List<BookDropdown>();
            }

            // ================= USERS =================

            // ================= USERS =================

            var usersResponse = await client.GetAsync("api/users");

            if (usersResponse.IsSuccessStatusCode)
            {
                var usersJson =
                    await usersResponse.Content.ReadAsStringAsync();

                var users =
                    JsonSerializer.Deserialize<List<UserDropdown>>
                    (
                        usersJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                model.Users =
                    users?
                    .Where(x => x.Role == "Student")
                    .ToList()
                    ?? new List<UserDropdown>();
            }
        }

        // =========================
        // RETURN BOOK
        // =========================
        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            var role = GetRole();

            if (role == "Student")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            var client = GetClient();

            await client.PostAsync(
                $"api/transactions/return/{id}",
                null);

            return RedirectToAction(nameof(Index));
        }
    }
}