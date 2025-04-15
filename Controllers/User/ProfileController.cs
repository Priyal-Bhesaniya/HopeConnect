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

            var user = _homeModel.GetUserByEmail(email); // Fetch user details by session email
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("Login", "Home");
            }

            return View("~/Views/User/Profile.cshtml", user); // Pass the fetched user to the view
        }

        [HttpPost]
        public IActionResult Profile(HomeModel updatedUser)
        {
            // Validate session to ensure user is logged in
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Home");

            var user = _homeModel.GetUserByEmail(email); // Get the original user from DB

            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("Login", "Home");
            }

            // Update values, email is not changed
            user.Name = updatedUser.Name;
            user.MobileNo = updatedUser.MobileNo;
            user.Password = updatedUser.Password;

            // Call method to update profile in DB
            bool isUpdated = _homeModel.UpdateUserProfile(user);

            if (isUpdated)
                TempData["Message"] = "Profile updated successfully.";
            else
                TempData["Message"] = "Profile update failed.";

            // Redirect to Profile page with the updated message
            return RedirectToAction("Profile");
        }
    }
}
