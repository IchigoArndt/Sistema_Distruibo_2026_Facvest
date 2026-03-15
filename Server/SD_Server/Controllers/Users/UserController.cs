using Microsoft.AspNetCore.Mvc;

namespace SD_Server.Api.Controllers.Users
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
