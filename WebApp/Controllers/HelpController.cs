using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
