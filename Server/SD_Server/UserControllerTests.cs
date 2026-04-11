using Xunit;
using Microsoft.AspNetCore.Mvc;
using SD_Server.Api.Controllers.Users; 

namespace SD_Server.Auth.Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _controller = new UserController();
        }

        [Fact]
        public void Login_ComCredenciaisValidas_DeveRetornarOkEToken()
        {
            var request = new LoginRequest { Email = "teste@ndd.com", Password = "123456" };

            var result = _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("token", okResult.Value.ToString().ToLower());
        }

        [Fact]
        public void Login_ComSenhaIncorreta_DeveRetornarUnauthorized()
        {
            var request = new LoginRequest { Email = "teste@ndd.com", Password = "senha_errada" };

            var result = _controller.Login(request);

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}