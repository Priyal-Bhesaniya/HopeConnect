using CurdNew.Models;
using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HopeConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeModel _homeModel;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _homeModel = new HomeModel();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Contactus()
        {
            return View();
        }

      

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string Name, string Email, string MobileNo, string Password)
        {
            // Create a new user from the submitted data
            var user = new HomeModel
            {
                Name = Name,
                Email = Email,
                MobileNo = MobileNo,
                Password = Password // Ensure you hash the password before storing in DB
            };

            // Save the user to the database (use service/repository pattern)
            bool isInserted = _homeModel.Insert(user);

            if (isInserted)
            {
                // Redirect to the Login page after successful registration
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Message = "Registration failed. Please try again.";
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
