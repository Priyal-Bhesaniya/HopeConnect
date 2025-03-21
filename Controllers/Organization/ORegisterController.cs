using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.Organization
{
    public class ORegisterController : Controller
    {
        public IActionResult ORegister()
        {
            ViewData["ActivePage"] = "ORegister";
            return View("~/Views/Organization/ORegister.cshtml");
        }
    }
}
