using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.Organization
{
    public class OallPostController : Controller
    {
        public IActionResult OallPost()
        {
            ViewData["ActivePage"] = "OallPost";
            return View("~/Views/Organization/OallPost.cshtml");
        }
    }
}
