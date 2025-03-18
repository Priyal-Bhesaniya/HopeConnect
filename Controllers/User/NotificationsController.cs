using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Notifications";
            return View("~/Views/User/Notifications.cshtml");
        }
    }
}
