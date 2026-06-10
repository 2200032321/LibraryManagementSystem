using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient()
        {
            var token = HttpContext.Session.GetString("JWToken");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri("https://localhost:7150/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public async Task<IActionResult> Index()
        {

            var role = HttpContext.Session.GetString("Role");

            if (role == "Student")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            var client = GetClient();

            var response = await client.GetAsync("api/users");


            if (!response.IsSuccessStatusCode)
                return View(new List<UserViewModel>());

            var json = await response.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<UserViewModel>>
            (
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id)
        {
            var client = GetClient();

            await client.PutAsync($"api/users/activate/{id}", null);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            var client = GetClient();

            await client.PutAsync($"api/users/deactivate/{id}", null);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetClient();

            await client.DeleteAsync($"api/users/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}