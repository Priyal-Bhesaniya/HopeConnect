using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class NetworkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
