using HopeConnect.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HopeConnect.Controllers.Organization
{
    public class OprofileController : Controller
    {
        private readonly OrganizationModel _organizationModel;

        // Use constructor injection for IConfiguration
        public OprofileController(IConfiguration configuration)
        {
            _organizationModel = new OrganizationModel(configuration); // Passing IConfiguration to OrganizationModel
        }

        // GET: Oprofile
        [HttpGet]
        public IActionResult Oprofile()
        {
            // Get OrgEmail from session
            var email = HttpContext.Session.GetString("OrgEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Home");

            // Fetch organization details using the email
            var organization = _organizationModel.GetByEmail(email);
            if (organization == null)
                return RedirectToAction("Login", "Home");

            // Pass the organization info to the view
            return View("~/Views/Organization/Oprofile.cshtml", organization);
        }

        // POST: Oprofile (for updating the organization details)
        [HttpPost]
        public IActionResult Oprofile(OrganizationModel updatedOrganization)
        {
            // Get OrgEmail from session
            var email = HttpContext.Session.GetString("OrgEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Home");

            // Fetch organization details using the email
            var organization = _organizationModel.GetByEmail(email);
            if (organization == null)
                return RedirectToAction("Login", "Home");

            // Update organization info
            organization.Name = updatedOrganization.Name;
            organization.MobileNo = updatedOrganization.MobileNo;
            organization.Password = updatedOrganization.Password;

            // Update in the database
            //bool isUpdated = _organizationModel.UpdateOrganizationProfile(organization);

            // Set a message for success/failure
            //TempData["Message"] = isUpdated ? "Profile updated successfully." : "Profile update failed.";

            return RedirectToAction("Oprofile");
        }
    }
}