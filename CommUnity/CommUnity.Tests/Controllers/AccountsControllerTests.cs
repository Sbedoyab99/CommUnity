//using CommUnity.BackEnd.Controllers;
//using CommUnity.BackEnd.Helpers;
//using CommUnity.BackEnd.UnitsOfWork.Interfaces;
//using CommUnity.Shared.DTOs;
//using CommUnity.Shared.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Moq;

//namespace CommUnity.Tests.Controllers
//{
//    [TestClass]
//    public class AccountsControllerTests
//    {
//        private Mock<IUsersUnitOfWork> _mockUsersUnitOfWork;
//        private Mock<IConfiguration> _mockConfiguration;
//        private Mock<IFileStorage> _mockFileStorage;
//        private Mock<IMailHelper> _mockMailHelper;
//        private AccountsController _controller;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockUsersUnitOfWork = new Mock<IUsersUnitOfWork>();
//            _mockConfiguration = new Mock<IConfiguration>();
//            _mockFileStorage = new Mock<IFileStorage>();
//            _mockMailHelper = new Mock<IMailHelper>();

//            _controller = new AccountsController(_mockUsersUnitOfWork.Object, _mockConfiguration.Object, _mockFileStorage.Object, _mockMailHelper.Object);
//        }

//        [TestMethod]
//        public async Task ResendTokenAsync_ReturnsNotFound_WhenEmailDoesNotExist()
//        {
//            var email = "emailtest@community.com";
//            var emailDTO = new EmailDTO { Email = email };

//            _mockUsersUnitOfWork.Setup(x => x.GetUserAsync(email)).ReturnsAsync((User)null);

//            var result = await _controller.ResedTokenAsync(emailDTO);

//            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
//            _mockUsersUnitOfWork.Verify(x => x.GetUserAsync(email), Times.Once);
//        }

//        [TestMethod]
//        public async Task ConfirmEmailAsync_ReturnsNotFound_WhenUserNotExists()
//        {
//            var userId = Guid.NewGuid().ToString();
//            var token = "valid-token";

//            _mockUsersUnitOfWork.Setup(x => x.GetUserAsync(new Guid(userId))).ReturnsAsync((User)null);

//            var result = await _controller.ConfirmEmailAsync(userId, token);

//            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
//            _mockUsersUnitOfWork.Verify(x => x.GetUserAsync(new Guid(userId)), Times.Once);
//        }

//        [TestMethod]
//        public async Task RecoverPasswordAsync_ReturnsNotFound_WhenEmailDoesNotExist()
//        {
//            var email = "emailtest@community.com";
//            var emailDTO = new EmailDTO { Email = email };

//            _mockUsersUnitOfWork.Setup(x => x.GetUserAsync(email)).ReturnsAsync((User)null);

//            var result = await _controller.RecoverPasswordAsync(emailDTO);

//            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
//            _mockUsersUnitOfWork.Verify(x => x.GetUserAsync(email), Times.Once);
//        }

//        [TestMethod]
//        public async Task ResetPasswordAsync_ReturnsNoContent_WhenPasswordReset()
//        {
//            var email = "emailtest@community.com";
//            var user = new User { Email = email };
//            var resetPasswordDTO = new ResetPasswordDTO { Email = email, Token = "token", Password = "new-password" };

//            _mockUsersUnitOfWork.Setup(x => x.GetUserAsync(email)).ReturnsAsync(user);
//            _mockUsersUnitOfWork.Setup(x => x.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password)).ReturnsAsync(IdentityResult.Success);

//            var result = await _controller.ResetPasswordAsync(resetPasswordDTO);

//            Assert.IsInstanceOfType(result, typeof(NoContentResult));
//            _mockUsersUnitOfWork.Verify(x => x.GetUserAsync(email), Times.Once);
//            _mockUsersUnitOfWork.Verify(x => x.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password), Times.Once);
//        }

//        [TestMethod]
//        public async Task ResetPasswordAsync_ReturnsNotFound_WhenEmailDoesNotExist()
//        {
//            var email = "emailtest@community.com";
//            var resetPasswordDTO = new ResetPasswordDTO { Email = email, Token = "token", Password = "new-password" };

//            _mockUsersUnitOfWork.Setup(x => x.GetUserAsync(email)).ReturnsAsync((User)null);

//            var result = await _controller.ResetPasswordAsync(resetPasswordDTO);

//            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
//            _mockUsersUnitOfWork.Verify(x => x.GetUserAsync(email), Times.Once);
//        }

//        [TestMethod]
//        public async Task ResetPasswordAsync_ReturnsBadRequest_WhenPasswordResetFails()
//        {
//            var email = "emailtest@community.com";
//            var user = new User { Email = email };
//            var resetPasswordDTO = new ResetPasswordDTO { Email = email, Token = "token", Password = "new-password" };

//            _mockUsersUnitOfWork.Setup(x => x.GetUserAsync(email)).ReturnsAsync(user);
//            _mockUsersUnitOfWork.Setup(x => x.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Reset failed" }));

//            var result = await _controller.ResetPasswordAsync(resetPasswordDTO);

//            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
//            var badRequestResult = result as BadRequestObjectResult;
//            Assert.AreEqual("Reset failed", badRequestResult.Value);
//            _mockUsersUnitOfWork.Verify(x => x.GetUserAsync(email), Times.Once);
//            _mockUsersUnitOfWork.Verify(x => x.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.Password), Times.Once);
//        }

//    }
//}
