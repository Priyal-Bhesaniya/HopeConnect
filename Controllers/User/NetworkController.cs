using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class NetworkController : Controller
    {
        public IActionResult Index() // Change "Network" to "Index" as per MVC convention
        {
            ViewData["ActivePage"] = "Network"; // Mark active page
            return View("~/Views/User/Network.cshtml"); // Explicitly point to the correct view
        }
    }
}
