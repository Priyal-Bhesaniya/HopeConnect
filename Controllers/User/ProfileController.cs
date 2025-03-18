using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
