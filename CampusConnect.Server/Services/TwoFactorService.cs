using OtpNet;
using QRCoder;
using CampusConnect.Server.Models;
using System;

namespace CampusConnect.Server.Services
{
    public class TwoFactorService
    {
        public string GenerateSecret()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        public string GenerateQrCodeUri(string secret, string userEmail, string issuer = "CampusConnect")
        {
            return $"otpauth://totp/{issuer}:{userEmail}?secret={secret}&issuer={issuer}";
        }

        public string GenerateQrCodeImage(string qrCodeUri)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(20);
            return Convert.ToBase64String(qrCodeBytes);
        }

        public bool ValidateCode(string secret, string code)
        {
            var secretBytes = Base32Encoding.ToBytes(secret);
            var totp = new Totp(secretBytes);
            return totp.VerifyTotp(code, out _);
        }
    }
}