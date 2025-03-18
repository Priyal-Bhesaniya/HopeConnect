using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class CreatePostController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "CreatePost";
            return View("~/Views/User/CreatePost.cshtml");
        }
    }
}

