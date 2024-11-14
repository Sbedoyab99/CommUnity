using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class UsersUnitOfWorkTests
    {
        private Mock<IUsersRepository> _mockUsersRepository = null!;
        private UsersUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockUsersRepository = new Mock<IUsersRepository>();
            _unitOfWork = new UsersUnitOfWork(_mockUsersRepository.Object);
        }

        [TestMethod]
        public async Task GeneratePasswordResetTokenAsync_CallsRepositoryAndReturnsToken()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var expectedToken = "resetToken";
            _mockUsersRepository.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(expectedToken);

            // Act
            var result = await _unitOfWork.GeneratePasswordResetTokenAsync(user);

            // Assert
            Assert.AreEqual(expectedToken, result);
            _mockUsersRepository.Verify(x => x.GeneratePasswordResetTokenAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task ResetPasswordAsync_CallsRepositoryAndReturnsIdentityResult()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var token = "resetToken";
            var password = "newPassword";
            var expectedResult = IdentityResult.Success;
            _mockUsersRepository.Setup(x => x.ResetPasswordAsync(user, token, password)).ReturnsAsync(expectedResult);

            // Act
            var result = await _unitOfWork.ResetPasswordAsync(user, token, password);

            // Assert
            Assert.AreEqual(expectedResult, result);
            _mockUsersRepository.Verify(x => x.ResetPasswordAsync(user, token, password), Times.Once);
        }

        [TestMethod]
        public async Task GenerateEmailConfirmationTokenAsync_CallsRepositoryAndReturnsToken()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var expectedToken = "confirmationToken";
            _mockUsersRepository.Setup(x => x.GenerateEmailConfirmationTokenAsync(user)).ReturnsAsync(expectedToken);

            // Act
            var result = await _unitOfWork.GenerateEmailConfirmationTokenAsync(user);

            // Assert
            Assert.AreEqual(expectedToken, result);
            _mockUsersRepository.Verify(x => x.GenerateEmailConfirmationTokenAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task ConfirmEmailAsync_CallsRepositoryAndReturnsIdentityResult()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var token = "confirmationToken";
            var expectedResult = IdentityResult.Success;
            _mockUsersRepository.Setup(x => x.ConfirmEmailAsync(user, token)).ReturnsAsync(expectedResult);

            // Act
            var result = await _unitOfWork.ConfirmEmailAsync(user, token);

            // Assert
            Assert.AreEqual(expectedResult, result);
            _mockUsersRepository.Verify(x => x.ConfirmEmailAsync(user, token), Times.Once);
        }

        [TestMethod]
        public async Task AddUserAsync_CallsRepositoryAndReturnsIdentityResult()
        {
            // Arrange
            var user = new User { Email = "newuser@example.com" };
            var password = "password123";
            var expectedResult = IdentityResult.Success;
            _mockUsersRepository.Setup(x => x.AddUserAsync(user, password)).ReturnsAsync(expectedResult);

            // Act
            var result = await _unitOfWork.AddUserAsync(user, password);

            // Assert
            Assert.AreEqual(expectedResult, result);
            _mockUsersRepository.Verify(x => x.AddUserAsync(user, password), Times.Once);
        }

        [TestMethod]
        public async Task AddUserToRoleAsync_CallsRepository()
        {
            // Arrange
            var user = new User { Email = "newuser@example.com" };
            var roleName = "Admin";
            _mockUsersRepository.Setup(x => x.AddUserToRoleAsync(user, roleName)).Returns(Task.CompletedTask);

            // Act
            await _unitOfWork.AddUserToRoleAsync(user, roleName);

            // Assert
            _mockUsersRepository.Verify(x => x.AddUserToRoleAsync(user, roleName), Times.Once);
        }

        [TestMethod]
        public async Task GetUserAsync_CallsRepositoryAndReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new User { Email = email };
            _mockUsersRepository.Setup(x => x.GetUserAsync(email)).ReturnsAsync(expectedUser);

            // Act
            var result = await _unitOfWork.GetUserAsync(email);

            // Assert
            Assert.AreEqual(expectedUser, result);
            _mockUsersRepository.Verify(x => x.GetUserAsync(email), Times.Once);
        }

        [TestMethod]
        public async Task IsUserInRoleAsync_CallsRepositoryAndReturnsTrue()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var roleName = "Admin";
            _mockUsersRepository.Setup(x => x.IsUserInRoleAsync(user, roleName)).ReturnsAsync(true);

            // Act
            var result = await _unitOfWork.IsUserInRoleAsync(user, roleName);

            // Assert
            Assert.IsTrue(result);
            _mockUsersRepository.Verify(x => x.IsUserInRoleAsync(user, roleName), Times.Once);
        }

        [TestMethod]
        public async Task GetUsersAsync_CallsRepositoryAndReturnsPagedUsers()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var role = UserType.Resident;
            var expectedResponse = new ActionResponse<IEnumerable<User>> { Result = new List<User> { new User { Email = "test@example.com" } } };
            _mockUsersRepository.Setup(x => x.GetUsersAsync(pagination, role)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetUsersAsync(pagination, role);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockUsersRepository.Verify(x => x.GetUsersAsync(pagination, role), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsTotalPages()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var role = UserType.Resident;
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockUsersRepository.Setup(x => x.GetTotalPagesAsync(pagination, role)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination, role);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockUsersRepository.Verify(x => x.GetTotalPagesAsync(pagination, role), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsRepositoryAndReturnsRecordCount()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var role = UserType.Resident;
            var expectedResponse = new ActionResponse<int> { Result = 100 };
            _mockUsersRepository.Setup(x => x.GetRecordsNumber(pagination, role)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination, role);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockUsersRepository.Verify(x => x.GetRecordsNumber(pagination, role), Times.Once);
        }

        [TestMethod]
        public async Task GetAdminResidentialUnit_CallsRepositoryAndReturnsUser()
        {
            // Arrange
            int residentialUnitId = 1;
            var expectedUser = new User { Email = "admin@example.com" };
            _mockUsersRepository.Setup(x => x.GetAdminResidentialUnit(residentialUnitId)).ReturnsAsync(new ActionResponse<User> { Result = expectedUser });

            // Act
            var result = await _unitOfWork.GetAdminResidentialUnit(residentialUnitId);

            // Assert
            Assert.AreEqual(expectedUser, result.Result);
            _mockUsersRepository.Verify(x => x.GetAdminResidentialUnit(residentialUnitId), Times.Once);
        }

        [TestMethod]
        public async Task LoginAsync_CallsRepositoryAndReturnsSignInResult()
        {
            // Arrange
            var loginModel = new LoginDTO { Email = "test@example.com", Password = "password123" };
            var expectedSignInResult = SignInResult.Success;
            _mockUsersRepository.Setup(x => x.LoginAsync(loginModel)).ReturnsAsync(expectedSignInResult);

            // Act
            var result = await _unitOfWork.LoginAsync(loginModel);

            // Assert
            Assert.AreEqual(expectedSignInResult, result);
            _mockUsersRepository.Verify(x => x.LoginAsync(loginModel), Times.Once);
        }

        [TestMethod]
        public async Task LogoutAsync_CallsRepository()
        {
            // Arrange
            _mockUsersRepository.Setup(x => x.LogoutAsync()).Returns(Task.CompletedTask);

            // Act
            await _unitOfWork.LogoutAsync();

            // Assert
            _mockUsersRepository.Verify(x => x.LogoutAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetUserAsync_ByGuid_CallsRepositoryAndReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User { Id = "123" };
            _mockUsersRepository.Setup(x => x.GetUserAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _unitOfWork.GetUserAsync(userId);

            // Assert
            Assert.AreEqual(expectedUser, result);
            _mockUsersRepository.Verify(x => x.GetUserAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task ChangePasswordAsync_CallsRepositoryAndReturnsIdentityResult()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var currentPassword = "oldPassword";
            var newPassword = "newPassword";
            var expectedResult = IdentityResult.Success;
            _mockUsersRepository.Setup(x => x.ChangePasswordAsync(user, currentPassword, newPassword)).ReturnsAsync(expectedResult);

            // Act
            var result = await _unitOfWork.ChangePasswordAsync(user, currentPassword, newPassword);

            // Assert
            Assert.AreEqual(expectedResult, result);
            _mockUsersRepository.Verify(x => x.ChangePasswordAsync(user, currentPassword, newPassword), Times.Once);
        }

        [TestMethod]
        public async Task UpdateUserAsync_CallsRepositoryAndReturnsIdentityResult()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var expectedResult = IdentityResult.Success;
            _mockUsersRepository.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(expectedResult);

            // Act
            var result = await _unitOfWork.UpdateUserAsync(user);

            // Assert
            Assert.AreEqual(expectedResult, result);
            _mockUsersRepository.Verify(x => x.UpdateUserAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task CheckRoleAsync_CallsRepository()
        {
            // Arrange
            var roleName = "Admin";
            _mockUsersRepository.Setup(x => x.CheckRoleAsync(roleName)).Returns(Task.CompletedTask);

            // Act
            await _unitOfWork.CheckRoleAsync(roleName);

            // Assert
            _mockUsersRepository.Verify(x => x.CheckRoleAsync(roleName), Times.Once);
        }
    }
}
