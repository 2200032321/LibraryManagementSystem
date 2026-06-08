using Microsoft.AspNetCore.Mvc;
namespace LibraryManagementSystem.WebApi.BackgroundServices
{
    public class EmailNotificationService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
