using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
