using Microsoft.AspNetCore.Mvc;
using TotpApplication.Service;

namespace TotpApplication.Controllers
{
    public class TotpController : Controller
    {
        public ActionResult Setup(string secretKey)
        {
            var uri = TotpHelper.GenerateQrCodeUri(secretKey, User.Identity.Name);
            var qrCodeImage = TotpHelper.GenerateQrCode(uri);
            ViewBag.QrCodeImage = Convert.ToBase64String(qrCodeImage);
            return View();
        }
    }
}
