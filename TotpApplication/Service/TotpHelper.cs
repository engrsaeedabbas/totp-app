using OtpNet;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using static QRCoder.PayloadGenerator;

namespace TotpApplication.Service
{
    public class TotpHelper
    {
        private const string _issuer = "TOTP_App";
        private const int _keyLength = 8;
        public static string GenerateSecretKey()
        {
            var key = KeyGeneration.GenerateRandomKey(_keyLength);
            return Base32Encoding.ToString(key);
        }

        public static string GenerateQrCodeImage(string secretKey, string account)
        {
            var uri = $"otpauth://totp/{_issuer}:{account}?secret={secretKey}&issuer={_issuer}&digits=6";

            using var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(5);

            return Convert.ToBase64String(qrCodeImage);
        }

        public static bool ValidateTotp(string secretKey, string code)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return totp.VerifyTotp(code, out long timeStepMatched, new VerificationWindow(2, 2));
        }
    }
}
