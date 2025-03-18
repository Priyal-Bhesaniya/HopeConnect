using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers
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
