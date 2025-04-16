using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace HopeConnect.Controllers.Organization
{
    public class AllPostApprovalController : Controller
    {
        public IActionResult AllPostApproval()
        {
            ViewData["ActivePage"] = "AllPostApproval";


            PostModel postModel = new PostModel();
            List<PostModel> posts = postModel.GetAllPosts();
            return View("~/Views/Organization/AllPostApproval.cshtml", posts);
        }
    }
}