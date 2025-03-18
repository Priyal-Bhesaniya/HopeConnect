using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers
{
    public class UserController : Controller
    {
        public IActionResult User()
        {
            return View();
        }
    }
}
