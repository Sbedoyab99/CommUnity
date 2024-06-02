using CommUnity.BackEnd.Controllers;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;

namespace CommUnity.Tests.Controllers
{
    [TestClass]
    public class VehiclesControllerTests
    {
        private Mock<IGenericUnitOfWork<Vehicle>> _mockGenericUnitOfWork;
        private Mock<IVehiclesUnitOfWork> _mockVehiclesUnitOfWork;
        private VehiclesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericUnitOfWork = new Mock<IGenericUnitOfWork<Vehicle>>();
            _mockVehiclesUnitOfWork = new Mock<IVehiclesUnitOfWork>();
            _controller = new VehiclesController(_mockGenericUnitOfWork.Object, _mockVehiclesUnitOfWork.Object);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<Vehicle>> { WasSuccess = true };
            _mockVehiclesUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockVehiclesUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<Vehicle>> { WasSuccess = false };
            _mockVehiclesUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockVehiclesUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<Vehicle>> { WasSuccess = true };
            _mockVehiclesUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockVehiclesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<Vehicle>> { WasSuccess = false };
            _mockVehiclesUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockVehiclesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = true, Result = 5 };
            _mockVehiclesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(action);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockVehiclesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = false };
            _mockVehiclesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(action);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockVehiclesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Id_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<Vehicle> { WasSuccess = true };
            _mockVehiclesUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockVehiclesUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Id_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<Vehicle> { WasSuccess = false, Message = "Vehicle not found" };
            _mockVehiclesUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(response.Message, notFoundResult!.Value);
            _mockVehiclesUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = true };
            _mockVehiclesUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockVehiclesUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockVehiclesUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockVehiclesUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }
    }
}
