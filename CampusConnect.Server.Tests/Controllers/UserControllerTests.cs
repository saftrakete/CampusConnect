using CampusConnect.Server.Controllers;
using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using CampusConnect.Server.Services;
using CampusConnect.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CampusConnect.Server.Tests.Controllers
{
    public class UserControllerTests : IDisposable
    {
        private readonly CampusConnectContext _context;
        private readonly Mock<IAuthorizationService> _mockAuthService;
        private readonly Mock<ILogger<ControllerBase>> _mockLogger;
        private readonly Mock<ITwoFactorService> _mockTwoFactorService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            var options = new DbContextOptionsBuilder<CampusConnectContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new CampusConnectContext(options);
            _mockAuthService = new Mock<IAuthorizationService>();
            _mockLogger = new Mock<ILogger<ControllerBase>>();
            _mockTwoFactorService = new Mock<ITwoFactorService>();
            
            _controller = new UserController(_context, _mockAuthService.Object, _mockLogger.Object, _mockTwoFactorService.Object);
        }

        [Fact]
        public async Task GetUserById_ExistingUser_ReturnsUser()
        {
            var user = new UserModel { UserId = 1, LoginName = "testuser" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.GetUserById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserModel>(okResult.Value);
            Assert.Equal("testuser", returnedUser.LoginName);
        }

        [Fact]
        public async Task GetUserById_NonExistingUser_ReturnsNotFound()
        {
            var result = await _controller.GetUserById(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task LoginNameAlreadyExists_ExistingUser_ReturnsTrue()
        {
            var user = new UserModel { UserId = 1, LoginName = "testuser" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.LoginNameAlreadyExists("testuser");

            Assert.True(result.Value);
        }

        [Fact]
        public async Task LoginNameAlreadyExists_NonExistingUser_ReturnsFalse()
        {
            var result = await _controller.LoginNameAlreadyExists("nonexistent");

            Assert.False(result.Value);
        }

        [Fact]
        public async Task UserLoginRequest_ValidCredentials_ReturnsToken()
        {
            var role = new UserRole { UserRoleId = 1, RoleName = "User" };
            var user = new UserModel { UserId = 1, LoginName = "testuser", Role = role };
            _context.UserRoles.Add(role);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginDto { LoginName = "testuser", Password = "password" };
            _mockAuthService.Setup(x => x.Authorize(It.IsAny<UserModel>(), It.IsAny<LoginDto>())).Returns(true);
            _mockAuthService.Setup(x => x.GenerateJwtToken(It.IsAny<UserModel>())).Returns("test-token");

            var result = await _controller.UserLoginRequest(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<LoginResponseDto>(okResult.Value);
            Assert.Equal("test-token", response.Token);
        }

        [Fact]
        public async Task UserLoginRequest_InvalidCredentials_ReturnsBadRequest()
        {
            var user = new UserModel { UserId = 1, LoginName = "testuser" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginDto { LoginName = "testuser", Password = "wrongpassword" };
            _mockAuthService.Setup(x => x.Authorize(It.IsAny<UserModel>(), It.IsAny<LoginDto>())).Returns(false);

            var result = await _controller.UserLoginRequest(loginDto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task VerifyTwoFactor_ValidCode_ReturnsSuccess()
        {
            var user = new UserModel { UserId = 1, LoginName = "testuser", TwoFactorSecret = "secret" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new TwoFactorVerifyDto { LoginName = "testuser", Code = "123456" };
            _mockTwoFactorService.Setup(x => x.ValidateCode("secret", "123456")).Returns(true);

            var result = await _controller.VerifyTwoFactor(dto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task VerifyTwoFactor_InvalidCode_ReturnsBadRequest()
        {
            var user = new UserModel { UserId = 1, LoginName = "testuser", TwoFactorSecret = "secret" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new TwoFactorVerifyDto { LoginName = "testuser", Code = "000000" };
            _mockTwoFactorService.Setup(x => x.ValidateCode("secret", "000000")).Returns(false);

            var result = await _controller.VerifyTwoFactor(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}