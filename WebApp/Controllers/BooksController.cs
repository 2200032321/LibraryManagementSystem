using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7150/");

            var token = GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        // ================= LIST (ALL ROLES) =================
        public async Task<IActionResult> Index()
        {
            var client = GetClient();

            var json = await client.GetStringAsync("api/books");

            var books = JsonSerializer.Deserialize<List<BookViewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(books ?? new List<BookViewModel>());
        }

        // ================= DETAILS (ALL ROLES) =================
        public async Task<IActionResult> Details(int id)
        {
            var client = GetClient();

            var json = await client.GetStringAsync($"api/books/{id}");

            var book = JsonSerializer.Deserialize<BookViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(book);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            if (!(IsAdmin() || IsLibrarian()))
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCreateViewModel model)
        {
            if (!(IsAdmin() || IsLibrarian()))
                return RedirectToAction("Index");

            var client = GetClient();

            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(model.Title ?? ""), "Title");
            form.Add(new StringContent(model.Author ?? ""), "Author");
            form.Add(new StringContent(model.ISBN ?? ""), "ISBN");
            form.Add(new StringContent(model.Publisher ?? ""), "Publisher");
            form.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(model.TotalCopies.ToString()), "TotalCopies");
            form.Add(new StringContent(model.PublishYear.ToString()), "PublishYear");
            form.Add(new StringContent(model.Description ?? ""), "Description");
            form.Add(new StringContent(model.IsEBook.ToString()), "IsEBook");

            if (model.CoverImageUrl != null)
            {
                var stream = model.CoverImageUrl.OpenReadStream();
                var file = new StreamContent(stream);
                file.Headers.ContentType =
                    new MediaTypeHeaderValue(model.CoverImageUrl.ContentType);

                form.Add(file, "CoverImage", model.CoverImageUrl.FileName);
            }

            var response = await client.PostAsync("api/books", form);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Failed";
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int id)
        {
            if (!(IsAdmin() || IsLibrarian()))
                return RedirectToAction("Index");

            var client = GetClient();

            var json = await client.GetStringAsync($"api/books/{id}");

            var book = JsonSerializer.Deserialize<BookCreateViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookCreateViewModel model)
        {
            if (!(IsAdmin() || IsLibrarian()))
                return RedirectToAction("Index");

            var client = GetClient();

            var json = JsonSerializer.Serialize(model);

            var response = await client.PutAsync(
                $"api/books/{id}",
                new StringContent(json, Encoding.UTF8, "application/json"));

            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index");

            var client = GetClient();

            await client.DeleteAsync($"api/books/{id}");

            return RedirectToAction("Index");
        }
    }
}