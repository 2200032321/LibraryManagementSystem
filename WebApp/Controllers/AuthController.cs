using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl = null)
        {
            var client = _clientFactory.CreateClient();

            var json = JsonSerializer.Serialize(model);

            var response = await client.PostAsync(
                "https://localhost:7150/api/auth/login",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid credentials";
                return View(model);
            }

            var result = await response.Content.ReadAsStringAsync();

            var login = JsonSerializer.Deserialize<LoginResponse>(result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (login?.Data == null)
            {
                ViewBag.Error = "Login failed";
                return View(model);
            }

            HttpContext.Session.SetString("JWToken", login.Data.Token);
            HttpContext.Session.SetString("Role", login.Data.Role);
            HttpContext.Session.SetString("UserName", login.Data.FullName);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}