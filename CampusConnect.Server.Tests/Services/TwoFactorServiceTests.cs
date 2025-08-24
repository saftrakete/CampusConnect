using CampusConnect.Server.Services;
using Xunit;

namespace CampusConnect.Server.Tests.Services
{
    public class TwoFactorServiceTests
    {
        private readonly TwoFactorService _service;

        public TwoFactorServiceTests()
        {
            _service = new TwoFactorService();
        }

        [Fact]
        public void GenerateSecret_ShouldReturnValidSecret()
        {
            var secret = _service.GenerateSecret();
            
            Assert.NotNull(secret);
            Assert.NotEmpty(secret);
            Assert.True(secret.Length >= 16);
        }

        [Fact]
        public void GenerateQrCodeUri_ShouldReturnValidUri()
        {
            var secret = "JBSWY3DPEHPK3PXP";
            var loginName = "testuser";
            
            var uri = _service.GenerateQrCodeUri(secret, loginName);
            
            Assert.NotNull(uri);
            Assert.Contains("otpauth://totp/", uri);
            Assert.Contains(loginName, uri);
            Assert.Contains(secret, uri);
        }

        [Fact]
        public void ValidateCode_WithValidCode_ShouldReturnTrue()
        {
            var secret = _service.GenerateSecret();
            var validCode = GenerateCurrentTotpCode(secret);
            
            var result = _service.ValidateCode(secret, validCode);
            
            Assert.True(result);
        }

        [Fact]
        public void ValidateCode_WithInvalidCode_ShouldReturnFalse()
        {
            var secret = _service.GenerateSecret();
            var invalidCode = "000000";
            
            var result = _service.ValidateCode(secret, invalidCode);
            
            Assert.False(result);
        }

        [Fact]
        public void GenerateQrCodeImage_ShouldReturnBase64String()
        {
            var uri = "otpauth://totp/test?secret=JBSWY3DPEHPK3PXP";
            
            var image = _service.GenerateQrCodeImage(uri);
            
            Assert.NotNull(image);
            Assert.NotEmpty(image);
        }

        private string GenerateCurrentTotpCode(string secret)
        {
            var totp = new OtpNet.Totp(OtpNet.Base32Encoding.ToBytes(secret));
            return totp.ComputeTotp();
        }
    }
}