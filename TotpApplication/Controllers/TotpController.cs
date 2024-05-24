using Microsoft.AspNetCore.Mvc;
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
    }
}
