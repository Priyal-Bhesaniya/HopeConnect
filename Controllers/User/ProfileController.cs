using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            ViewData["ActivePage"] = "Profile";
            return View();
        }
    }
}
