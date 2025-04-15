using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using CurdNew.Models;

namespace HopeConnect.Controllers.User
{
    public class CreatePostController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "CreatePost";
            ViewBag.UserName = TempData["UserName"] ?? "User"; // ✅ set ViewBag before return
            TempData.Keep("UserName"); // keep for next request
            return View("~/Views/User/CreatePost.cshtml");
        }

        [HttpPost]
        public IActionResult SubmitPost(string thoughts, string organization, double latitude, double longitude, string locationName, IFormFile photo)
        {
            string email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Home");

            var homeModel = new HomeModel();
            var user = homeModel.GetUserByEmail(email);
            if (user == null)
                return RedirectToAction("Login", "Home");

            string filePath = null;
            if (photo != null && photo.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                filePath = "/uploads/" + uniqueFileName;
                using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
            }

            var post = new PostModel
            {
                UserId = user.UserId,
                ThoughtText = thoughts,
                Organization = organization,
                PhotoPath = filePath,
                Latitude = latitude,
                Longitude = longitude,
                LocationName = locationName
            };

            bool result = new PostModel().Insert(post);
            TempData["Message"] = result ? "Post created!" : "Something went wrong.";

            return RedirectToAction("Index");
        }
    }
}
