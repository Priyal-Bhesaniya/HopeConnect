using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class NotificationsController : Controller
    {
        public IActionResult Notifications()
        {
            ViewData["ActivePage"] = "Notifications";
            return View();
        }
    }
}
