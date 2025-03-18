using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class UserController : Controller
    {
        public IActionResult User()
        {
            ViewData["ActivePage"] = "AllPost";
            return View();
        }



    }
}
