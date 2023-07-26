using System;
using System.Threading.Tasks;
using Bandit.ACS.Daemon.Models.DTOs;
using Bandit.ACS.Daemon.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Noa.SectorMapper.Docker.Controllers.v1;

namespace Bandit.ACS.ComponentTest.Tests.Controllers
{
    [TestFixture]
    public class AccountsControllerTest
    {
        private Mock<IAccountsService> _mockAccountsService;
        private Mock<ITokenService> _mockTokenService;
        private AccountsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAccountsService = new Mock<IAccountsService>();
            _mockTokenService = new Mock<ITokenService>();
            _controller = new AccountsController(_mockAccountsService.Object, _mockTokenService.Object)
            {
                Logger = new NullLogger<AccountsController>()
            };
        }


        [Test]
        public async Task GetProfile_ReturnsOkObjectResult_WithProfileDTO()
        {
            // Arrange
            var expectedProfile = new ProfileDTO
            {
                Account = new Bandit.ACS.Daemon.Models.Account
                {
                    Id = Guid.NewGuid(),
                    Mail = "test@example.com",
                    FirstName = "Jamy",
                    LastName = "Dupond",
                    BirthDay = new DateTime(2000, 1, 1),
                    Gender = "Male",
                    IsAdmin = false
                },
                Card = new Bandit.ACS.Daemon.Models.Card
                {
                    OwnerId = Guid.NewGuid(),
                    CardNumber = "1234567890123456",
                    Balance = 100.00
                }
            };
            var token = "test-token";
            _mockTokenService.Setup(x => x.GetAccountAsync(token)).ReturnsAsync(new Bandit.ACS.Daemon.Models.Account { Id = expectedProfile.Account.Id });
            _mockAccountsService.Setup(x => x.GetProfile(expectedProfile.Account.Id)).ReturnsAsync(expectedProfile);

            // Act
            var result = await _controller.GetProfile();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<ProfileDTO>(okResult.Value);
            var actualProfile = okResult.Value as ProfileDTO;
            Assert.AreEqual(expectedProfile, actualProfile);
        }
    }
}
