using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected string? GetToken()
            => HttpContext.Session.GetString("JWToken");

        protected string? GetRole()
            => HttpContext.Session.GetString("Role");

        protected string? GetUserName()
            => HttpContext.Session.GetString("UserName");

        protected bool IsAdmin()
            => GetRole() == "Admin";

        protected bool IsLibrarian()
            => GetRole() == "Librarian";

        protected bool IsStudent()
            => GetRole() == "Student";
    }
}