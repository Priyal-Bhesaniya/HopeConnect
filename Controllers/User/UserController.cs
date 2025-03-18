using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class UserController : Controller
    {
        public IActionResult Network()
        {
            ViewData["ActivePage"] = "Network";
            return View();
        }



    }
}
