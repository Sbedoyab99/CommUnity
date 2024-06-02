using CommUnity.BackEnd.Controllers;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CommUnity.Tests.Controllers
{
    [TestClass]
    public class CommonZonesControllerTests
    {
        private Mock<IGenericUnitOfWork<CommonZone>> _mockGenericUnitOfWork;
        private Mock<ICommonZonesUnitOfWork> _mockCommonZonesUnitOfWork;
        private CommonZonesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericUnitOfWork = new Mock<IGenericUnitOfWork<CommonZone>>();
            _mockCommonZonesUnitOfWork = new Mock<ICommonZonesUnitOfWork>();
            _controller = new CommonZonesController(_mockGenericUnitOfWork.Object, _mockCommonZonesUnitOfWork.Object);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<CommonZone>> { WasSuccess = true };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockCommonZonesUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<CommonZone>> { WasSuccess = false };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockCommonZonesUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var commonZones = new List<CommonZone> { new CommonZone { Id = 1, Name = "Zone 1" } };
            var response = new ActionResponse<IEnumerable<CommonZone>> { WasSuccess = true, Result = commonZones };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(commonZones, okResult.Value);
            _mockCommonZonesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<CommonZone>> { WasSuccess = false };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockCommonZonesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = true, Result = 5 };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(action);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockCommonZonesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = false };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(action);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockCommonZonesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsyncById_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<CommonZone> { WasSuccess = true };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockCommonZonesUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetAsyncById_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<CommonZone> { WasSuccess = false, Message = "Not found" };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(response.Message, notFoundResult!.Value);
            _mockCommonZonesUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = true, Result = 10 };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockCommonZonesUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockCommonZonesUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockCommonZonesUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }
    }
}
