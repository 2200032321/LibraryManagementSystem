using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationsController(
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
            var client = GetClient();

            var response =
                await client.GetAsync("api/notifications");

            if (!response.IsSuccessStatusCode)
            {
                return View(
                    new List<NotificationViewModel>());
            }

            var json =
                await response.Content.ReadAsStringAsync();

            using var doc =
                JsonDocument.Parse(json);

            var data =
                doc.RootElement.GetProperty("data");

            var notifications =
                JsonSerializer.Deserialize<
                    List<NotificationViewModel>>
                (
                    data.GetRawText(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

            return View(
                notifications ??
                new List<NotificationViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(int id)
        {
            var client = GetClient();

            await client.PutAsync(
                $"api/notifications/read/{id}",
                null);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetClient();

            await client.DeleteAsync(
                $"api/notifications/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}