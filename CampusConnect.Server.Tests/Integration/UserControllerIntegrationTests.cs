using System.Net;
using System.Net.Http.Json;
using Xunit;
using CampusConnect.Server.Models.Dtos;

namespace CampusConnect.Server.Tests.Integration
{
    public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_MissingCredentials_ReturnsBadRequest()
        {
            var loginDto = new LoginDto { LoginName = "", Password = "" };
            var response = await _client.PostAsJsonAsync("/user/login", loginDto);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_NonExistentUser_ReturnsNotFound()
        {
            var loginDto = new LoginDto { LoginName = "nonexistent", Password = "password" };
            var response = await _client.PostAsJsonAsync("/user/login", loginDto);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/user/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task LoginNameExists_NonExistingUser_ReturnsFalse()
        {
            var response = await _client.GetAsync("/user/exists/nonexistent");
            var exists = await response.Content.ReadFromJsonAsync<bool>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(exists);
        }
    }
}