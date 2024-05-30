using Microsoft.AspNetCore.Mvc;
using TotpApplication.Models;

namespace TotpApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Session session = Session.GetInstance();
            User currentUser = session.CurrentUser;

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = "No signed user in Credentials found";
                return RedirectToAction("Login", "Totp");
            }

            if (currentUser.IsCodeValidated == false && currentUser.IsTotpEnabled == true)
            {
                return RedirectToAction("CodeValidation", "Totp");
            }
            return View();
        }
        
        [HttpGet]
        public ActionResult MyAccount()
        {
            Session session = Session.GetInstance();
            User currentUser = session.CurrentUser;

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = "No signed user in Credentials found";
                return View("Login", "Totp");
            }

            ViewBag.CurrentUser = currentUser;
            ViewBag.Username = currentUser.Name;
            return View();
        }

    }
}
