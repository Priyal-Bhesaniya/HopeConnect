using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.Organization
{
    public class OallPostController : Controller
    {
        public IActionResult OallPost()



        {
            ViewData["ActivePage"] = "OallPost";
            PostModel postModel = new PostModel();
            List<PostModel> posts = postModel.GetAllPosts();
            return View("~/Views/Organization/OallPost.cshtml", posts);




        }
    }
}