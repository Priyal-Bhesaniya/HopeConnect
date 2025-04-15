using CurdNew.Models;
using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HopeConnect.Controllers.User
{
    public class ProfileController : Controller
    {
        private readonly HomeModel _homeModel;

        public ProfileController()
        {
            _homeModel = new HomeModel();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Home");

            var user = _homeModel.GetUserByEmail(email); // Get user info
            if (user == null)
                return RedirectToAction("Login", "Home");

            // Fetch posts created by the user
            var postModel = new PostModel();
            var userPosts = postModel.GetPostsByUserId(user.UserId);
            ViewBag.Posts = userPosts;

            return View("~/Views/User/Profile.cshtml", user);
        }

        [HttpPost]
        public IActionResult Profile(HomeModel updatedUser)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Home");

            var user = _homeModel.GetUserByEmail(email);
            if (user == null)
                return RedirectToAction("Login", "Home");

            user.Name = updatedUser.Name;
            user.MobileNo = updatedUser.MobileNo;
            user.Password = updatedUser.Password;

            bool isUpdated = _homeModel.UpdateUserProfile(user);
            TempData["Message"] = isUpdated ? "Profile updated successfully." : "Profile update failed.";

            return RedirectToAction("Profile");
        }
    }
}
