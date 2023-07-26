using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.Daemon.Models;
using Bandit.ACS.Daemon.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Noa.SectorMapper.Docker.Controllers.v1;
using NUnit.Framework;

namespace Bandit.ACS.Daemon.ComponentTest.CommandHandlers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAccountsService> _accountsServiceMock = new Mock<IAccountsService>();
        private readonly Mock<IChallengeService> _challengeServiceMock = new Mock<IChallengeService>();
        private readonly Mock<ITokenService> _tokenServiceMock = new Mock<ITokenService>();
        private AuthenticationController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new AuthenticationController(_accountsServiceMock.Object, _challengeServiceMock.Object, _tokenServiceMock.Object);
        }

        [Test]
        public async Task Login_ReturnsOkObjectResult_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginDTO = new LoginDTO();
            var token = new SessionTokenDTO();
            _accountsServiceMock.Setup(x => x.Login(loginDTO, It.IsAny<string>())).ReturnsAsync(token);

            // Act
            var result = await _controller.Login(loginDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(token, (result as OkObjectResult).Value);
        }

        [Test]
        public async Task Register_ReturnsOkObjectResult_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerDto = new RegisterDTO();
            var token = new SessionTokenDTO();
            _accountsServiceMock.Setup(x => x.Register(registerDto, It.IsAny<string>())).ReturnsAsync(token);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(token, (result as OkObjectResult).Value);
        }

        [Test]
        public async Task OTPChallengeRequest_ReturnsOkObjectResult_WhenChallengeIsGenerated()
        {
            // Arrange
            var token = "access_token";
            var challenger = new Account();
            var challenge = new ChallengeDTO();
            _tokenServiceMock.Setup(x => x.GetAccountAsync(token)).ReturnsAsync(challenger);
            _challengeServiceMock.Setup(x => x.GenerateChallengeAsync(challenger, It.IsAny<string>(), Bandit.ACS.Daemon.Models.ChallengeType.OTP)).ReturnsAsync(challenge);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer " + token;
            _controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await _controller.OTPChallengeRequest();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(challenge, (result as OkObjectResult).Value);
        }

        [Test]
        public async Task OTPChallengeAttempt_ReturnsOkObjectResult_WhenChallengeAttemptIsSuccessful()
        {
            // Arrange
            var token = "access_token";
            var challenger = new Account();
            var attemptResult = new AttemptResult();
            var attempt = new ChallengeAttemptDTO();
            _tokenServiceMock.Setup(x => x.GetAccountAsync(token)).ReturnsAsync(challenger);
            _challengeServiceMock.Setup(x => x.AttemptAsync(attempt, challenger, It.IsAny<string>())).ReturnsAsync(attemptResult);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer " + token;
            _controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await _controller.OTPChallengeAttempt(attempt);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(attemptResult, (result as OkObjectResult).Value);
        }
    }

}
