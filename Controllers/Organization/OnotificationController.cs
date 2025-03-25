using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.Organization
{
    public class OnotificationController : Controller
    {
        public IActionResult Onotification()
        {
            ViewData["ActivePage"] = "Onotification";
            return View("~/Views/Organization/Onotification.cshtml");
        }
    }
}
