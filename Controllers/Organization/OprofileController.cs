using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.Organization
{
    public class OprofileController : Controller
    {
        public IActionResult Oprofile()
        {
            ViewData["ActivePage"] = "Oprofile";
            return View("~/Views/Organization/Oprofile.cshtml");
        }
    }
}
