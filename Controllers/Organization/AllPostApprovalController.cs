using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace HopeConnect.Controllers.Organization
{
    public class AllPostApprovalController : Controller
    {
        public IActionResult OPostApproval()
        {
            ViewData["ActivePage"] = "OPostApproval";
            return View("~/Views/Organization/AllPostApproval.cshtml");
        }
    }
}
