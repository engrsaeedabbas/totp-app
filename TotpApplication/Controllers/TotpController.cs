using Microsoft.AspNetCore.Mvc;
using TotpApplication.Service;

namespace TotpApplication.Controllers
{
    public class TotpController : Controller
    {
        public ActionResult Setup(string id)
        {
            var uri = TotpHelper.GenerateQrCodeUri(id, User.Identity.Name);
            var qrCodeImage = TotpHelper.GenerateQrCode(uri);
            ViewBag.QrCodeImage = Convert.ToBase64String(qrCodeImage);
            return View();
        }
    }
}
