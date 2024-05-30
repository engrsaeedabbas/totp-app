using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using TotpApplication.Models;
using TotpApplication.Service;

namespace TotpApplication.Controllers
{
    public class TotpController : Controller
    {
        private const string _secretKey = "MYEQW3UI4I22I===";
        private const string _user = "abbas.saeed.engr@gmail.com";

        [HttpGet]
        public ActionResult Setup(string id)
        {
            var key = GetKey(id);
            ViewBag.IsValidated = false;
            ViewBag.QrCodeImage = TotpHelper.GenerateQrCodeImage(key, _user);
            return View();
        }

        [HttpPost]
        public ActionResult Setup(string id, string code)
        {
            var key = GetKey(id);
            ViewBag.IsValidated = true;
            ViewBag.IsTotpValidated = TotpHelper.ValidateTotp(key, code);
            ViewBag.QrCodeImage = TotpHelper.GenerateQrCodeImage(key, _user);
            return View();
        }

        private static string GetKey(string id)
        {
            var key = _secretKey;
            if (!string.IsNullOrEmpty(id))
            {
                key = id;
            }
            return key;
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            Session session = Session.GetInstance();
            //session.CurrentUser.IsCodeValidated = false;
            session.CurrentUser = null;
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewAccount(string name, string email, string password)
        {
            // Validation is handled in the client-side jQuery code

            Session session = Session.GetInstance();
            var user = session.AllUsers.Any(u => u.Email == email);

            if (!user)
            {
                User newUser = new User
                {
                    Id = session.AllUsers.Count + 1, // Assign a unique ID
                    Name = name,
                    Email = email,
                    Password = password,
                    SecretKey = null,
                    IsTotpEnabled = false
                };

                session.AllUsers.Add(newUser);
                return Json(new { success = true, redirectUrl = Url.Action("Login") });
            }

            return Json(new { success = false, message = "User with the same email exists" });
        }

        [HttpGet]
        public ActionResult Login()
        {
            Session session = Session.GetInstance();
            //session.CurrentUser.IsCodeValidated = true? false : false;
            session.CurrentUser = null;
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit(string email, string password)
        {
            Session session = Session.GetInstance();
            User user = session.AllUsers.Where(u => u.Email == email && u.Password == password).FirstOrDefault();

            if (user == null)
            {
                ViewBag.ErrorMessage = "No user Found with such credentials";
                return View("Login");
            }

            session.CurrentUser = user;

            if(session.CurrentUser.IsTotpEnabled)
            {
                return RedirectToAction("CodeValidation", "Totp");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear session variables or perform other logout actions
            Session session = Session.GetInstance();
            session.CurrentUser.IsCodeValidated = false;

            // Redirect to login page or any other page after logout
            return RedirectToAction("Login", "Totp");
        }

        [HttpGet]
        public ActionResult CodeValidation()
        {
            Session session = Session.GetInstance();
            User currentUser = session.CurrentUser;

            if (currentUser == null)
            {
                return Json(new { success = false, message = "No user details were found. Please sign in again!" });
            }

            if (currentUser.IsTotpEnabled && string.IsNullOrEmpty(currentUser.SecretKey) && string.IsNullOrEmpty(currentUser.Name))
            {
                return Json(new { success = false, message = "Some of the user details are not found. Please sign in or create a new account again!" });
            }

            bool newSecretKey = string.IsNullOrEmpty(currentUser.SecretKey) && !currentUser.IsTotpEnabled;

            if (newSecretKey)
            {
                var sKey = TotpHelper.GenerateSecretKey();
                var qrCodeImage = TotpHelper.GenerateQrCodeImage(sKey, currentUser.Name);
                return View(new { IsNewSecretKey = true, SecretKey = sKey.ToString(), QrCodeImage = qrCodeImage, IsValidated = (bool?)null, IsTotpValidated = false });
            }

            return View(new { IsNewSecretKey = false, SecretKey = currentUser.SecretKey, IsValidated = (bool?)null, IsTotpValidated = false });
        }


        [HttpPost]
        public ActionResult CodeValidation(string sKey, string code)
        {
            if (string.IsNullOrEmpty(sKey))
            {
                return Json(new { success = false, message = "Can not get the secret key.", IsValidated = false, IsTotpValidated = false });
            }

            if (string.IsNullOrEmpty(code))
            {
                return Json(new { success = false, message = "Invalid code. Please re-enter the code.", IsValidated = false, IsTotpValidated = false });
            }

            Session session = Session.GetInstance();
            User currentUser = session.CurrentUser;

            if (TotpHelper.ValidateTotp(sKey, code))
            {
                currentUser.SecretKey = sKey;
                currentUser.IsTotpEnabled = true;
                currentUser.IsCodeValidated = true;
                return Json(new { success = true, message = "Code Validated Successfully", IsValidated = true, IsTotpValidated = true, redirectUrl = Url.Action("Index", "Home") });
            }

            return Json(new { success = false, message = "Code Validation Failed", IsValidated = true, IsTotpValidated = false });
        }

    }
}
