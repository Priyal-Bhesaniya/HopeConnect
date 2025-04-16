using HopeConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

namespace HopeConnect.Controllers.Organization
{
    public class ORegisterController : Controller
    {
        private readonly OrganizationModel _orgModel;
        private readonly EmailSettings _emailSettings;

        public ORegisterController(IConfiguration configuration, IOptions<EmailSettings> emailSettings)
        {
            _orgModel = new OrganizationModel(configuration);
            _emailSettings = emailSettings.Value;
        }

        // Show the Registration Form
        [HttpGet]
        public IActionResult ORegister()
        {
            ViewData["ActivePage"] = "ORegister";
            return View("~/Views/Organization/ORegister.cshtml");
        }

        // Handle Registration Submission
        [HttpPost]
        public IActionResult Register(string Name, string Email, string MobileNo, string Password, string Type)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Name, @"^[a-zA-Z\s]+$"))
            {
                ViewBag.Message = "Name should contain only letters.";
                return View("~/Views/Organization/ORegister.cshtml");
            }

            try { var emailCheck = new MailAddress(Email); }
            catch
            {
                ViewBag.Message = "Invalid email format.";
                return View("~/Views/Organization/ORegister.cshtml");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(MobileNo, @"^\d{10}$"))
            {
                ViewBag.Message = "Mobile number must be 10 digits.";
                return View("~/Views/Organization/ORegister.cshtml");
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6 ||
                !System.Text.RegularExpressions.Regex.IsMatch(Password, @"^(?=.*[0-9!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$"))
            {
                ViewBag.Message = "Password must be at least 6 characters and include a number or symbol.";
                return View("~/Views/Organization/ORegister.cshtml");
            }

            string token = Guid.NewGuid().ToString();

            var org = new OrganizationModel(null)
            {
                Name = Name,
                Email = Email,
                MobileNo = MobileNo,
                Password = Password,
                Type = Type,
                IsEmailVerified = false,
                EmailVerificationToken = token
            };

            bool inserted = _orgModel.Insert(org);

            if (inserted)
            {
                string verificationUrl = Url.Action("VerifyEmail", "ORegister", new { token = token }, Request.Scheme);
                string subject = "Verify your organization email";
                string body = $"Click the link to verify your email: <a href='{verificationUrl}'>Verify Email</a>";

                SendEmail(Email, subject, body);

                TempData["Message"] = "Registration successful! Check your email to verify.";
                return RedirectToAction("OLogin");
            }

            ViewBag.Message = "Registration failed. Try again.";
            return View("~/Views/Organization/ORegister.cshtml");
        }

        [HttpGet]
        public IActionResult OLogin()
        {
            return View("~/Views/Organization/OLogin.cshtml");
        }

        [HttpPost]
        public IActionResult OLogin(string Email, string Password)
        {
            var org = _orgModel.GetByEmailAndPassword(Email, Password);

            if (org != null && org.IsEmailVerified)
            {
                HttpContext.Session.SetString("OrgEmail", Email);
                return RedirectToAction("OallPost", "OallPost");
            }

            TempData["Message"] = "Invalid credentials or email not verified.";
            return RedirectToAction("OLogin");
        }

        public IActionResult VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Message"] = "Invalid token.";
                return RedirectToAction("OLogin");
            }

            bool verified = _orgModel.VerifyEmail(token);

            TempData["Message"] = verified
                ? "Email verified successfully. You can now log in."
                : "Invalid or expired verification link.";

            return RedirectToAction("OLogin");
        }

        public IActionResult OrgProfile()
        {
            string email = HttpContext.Session.GetString("OrgEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("OLogin");

            var org = _orgModel.GetByEmailAndPassword(email, null);
            return View("~/Views/Organization/OrgProfile.cshtml", org);
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
    }
}