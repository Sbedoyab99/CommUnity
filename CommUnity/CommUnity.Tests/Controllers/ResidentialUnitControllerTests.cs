using Microsoft.AspNetCore.Mvc;
using Moq;
using CommUnity.BackEnd.Controllers;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.Controllers
{
    [TestClass]
    public class ResidentialUnitControllerTests
    {
        private Mock<IGenericUnitOfWork<ResidentialUnit>> _mockGenericUnitOfWork = null!;
        private Mock<IResidentialUnitUnitOfWork> _mockResidentialUnitUnitOfWork = null!;
        private ResidentialUnitController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericUnitOfWork = new Mock<IGenericUnitOfWork<ResidentialUnit>>();
            _mockResidentialUnitUnitOfWork = new Mock<IResidentialUnitUnitOfWork>();
            _controller = new ResidentialUnitController(_mockGenericUnitOfWork.Object, _mockResidentialUnitUnitOfWork.Object);
        }

        [TestMethod]
        public async Task GetAsync_Full_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<ResidentialUnit>> { WasSuccess = true };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Full_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<ResidentialUnit>> { WasSuccess = false };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<ResidentialUnit>> { WasSuccess = true };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<ResidentialUnit>> { WasSuccess = false };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = true, Result = 5 };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(action);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = false };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(action);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<ResidentialUnit> { WasSuccess = true, Result = new ResidentialUnit() };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<ResidentialUnit> { WasSuccess = false, Message = "Not found" };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(response.Message, notFoundResult!.Value);
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = true, Result = 100 };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockResidentialUnitUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockResidentialUnitUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }

    }
}
