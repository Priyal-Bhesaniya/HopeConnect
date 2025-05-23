﻿using CurdNew.Models;
using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace HopeConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeModel _homeModel;
        private readonly EmailSettings _emailSettings;

        public HomeController(ILogger<HomeController> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _homeModel = new HomeModel();
            _emailSettings = emailSettings.Value;
        }

        public IActionResult Index() => View();
        public IActionResult Aboutus() => View();
        public IActionResult Contactus() => View();
        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            var user = _homeModel.GetUserByEmailAndPassword(Email, Password);

            if (user != null)
            {
                TempData["Message"] = "Login successful!";
                HttpContext.Session.SetString("UserEmail", Email); // Save email in session
                return RedirectToAction("Profile", "Profile");
            }

            TempData["Message"] = "Invalid credentials or email not verified.";
            return RedirectToAction("Login");
        }
        public IActionResult User()
        {
            return View(); // create User.cshtml page to show user info
        }



        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string Name, string Email, string MobileNo, string Password)
        {
            // Name should contain only letters
            if (!System.Text.RegularExpressions.Regex.IsMatch(Name, @"^[a-zA-Z\s]+$"))
            {
                ViewBag.Message = "Name should contain only letters.";
                return View();
            }

            // Email format check
            try
            {
                var emailCheck = new MailAddress(Email);
            }
            catch
            {
                ViewBag.Message = "Invalid email format.";
                return View();
            }

            // MobileNo must be 10 digits
            if (!System.Text.RegularExpressions.Regex.IsMatch(MobileNo, @"^\d{10}$"))
            {
                ViewBag.Message = "Mobile number must be 10 digits.";
                return View();
            }

            // Password must be at least 6 characters and contain at least one number or symbol
            if (string.IsNullOrWhiteSpace(Password) ||
                Password.Length < 6 ||
                !System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[0-9!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$"))
            {
                ViewBag.Message = "Password must be at least 6 characters and include a number or symbol.";
                return View();
            }

            // Token generation for email verification
            var token = Guid.NewGuid().ToString();

            var user = new HomeModel
            {
                Name = Name,
                Email = Email,
                MobileNo = MobileNo,
                Password = Password,
                IsEmailVerified = false,
                EmailVerificationToken = token
            };

            bool isInserted = _homeModel.Insert(user);

            if (isInserted)
            {
                string verificationUrl = Url.Action("VerifyEmail", "Home", new { token = token }, Request.Scheme);
                string subject = "Verify your email";
                string body = $"Click the link to verify your email: <a href='{verificationUrl}'>Verify Email</a>";

                SendEmail(Email, subject, body);

                TempData["Message"] = "Registration successful! Please check your email to verify your account.";
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Registration failed. Please try again.";
            return View();
        }

        public IActionResult VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Message"] = "Invalid token.";
                return RedirectToAction("Login");
            }

            var user = _homeModel.GetUserByToken(token);

            if (user != null)
            {
                user.IsEmailVerified = true;
                bool isUpdated = _homeModel.UpdateEmailVerification(user);

                if (isUpdated)
                    TempData["Message"] = "Email verified successfully. You can now log in.";
                else
                    TempData["Message"] = "Error while verifying your email.";
            }
            else
            {
                TempData["Message"] = "Invalid or expired verification link.";
            }

            // ✅ Always redirect to Login after clicking the link
            return RedirectToAction("Login");
        }




        private void SendEmail(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);

                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, "Hope Connect"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                client.Send(mail);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}