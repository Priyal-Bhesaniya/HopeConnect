using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.User
{
    public class CreatePostController : Controller
    {
        public IActionResult CreatePost()
        {
            ViewData["ActivePage"] = "CreatePost";
            return View();
        }
    }
}
