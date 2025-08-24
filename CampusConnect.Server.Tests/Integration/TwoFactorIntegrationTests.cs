using System.Net;
using System.Net.Http.Json;
using Xunit;
using CampusConnect.Server.Models.Dtos;

namespace CampusConnect.Server.Tests.Integration
{
    public class TwoFactorIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TwoFactorIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task SetupTwoFactor_NonExistingUser_ReturnsNotFound()
        {
            var request = new TwoFactorVerifyDto { LoginName = "nonexistent" };
            var response = await _client.PostAsJsonAsync("/user/2fa/setup", request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task VerifyTwoFactor_MissingData_ReturnsBadRequest()
        {
            var request = new TwoFactorVerifyDto { LoginName = "", Code = "" };
            var response = await _client.PostAsJsonAsync("/user/2fa/verify", request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}