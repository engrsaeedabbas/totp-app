using OtpNet;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using static QRCoder.PayloadGenerator;

namespace TotpApplication.Service
{
    public class TotpHelper
    {
        public static string GenerateSecretKey()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        public static string GenerateQrCodeUri(string secretKey, string account)
        {
            string issuer = "YourAppName";
            return $"otpauth://totp/{issuer}:{account}?secret={secretKey}&issuer={issuer}&digits=6";
        }

        public static byte[] GenerateQrCode(string uri)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(5);

            return qrCodeImage;
        }

        public static bool ValidateTotp(string secretKey, string code)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return totp.VerifyTotp(code, out long timeStepMatched, new VerificationWindow(2, 2));
        }
    }
}
