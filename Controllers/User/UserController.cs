using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HopeConnect.Controllers.User
{
    public class UserController : Controller
    {
        public IActionResult User()
        {
            ViewData["ActivePage"] = "AllPost";

            PostModel postModel = new PostModel();
            List<PostModel> posts = postModel.GetAllPosts();

            return View(posts);
        }
    }
}
