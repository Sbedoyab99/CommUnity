using CommUnity.BackEnd.Controllers;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CommUnity.Tests.Controllers
{
    [TestClass]
    public class GenericControllerTests
    {
        private Mock<IGenericUnitOfWork<TestEntity>> _mockUnitOfWork;
        private GenericController<TestEntity> _controller;

        public class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IGenericUnitOfWork<TestEntity>>();
            _controller = new GenericController<TestEntity>(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<TestEntity>> { WasSuccess = true };
            _mockUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<TestEntity>> { WasSuccess = false };
            _mockUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<TestEntity>> { WasSuccess = true };
            _mockUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<TestEntity>> { WasSuccess = false };
            _mockUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = true, Result = 5 };
            _mockUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsyncById_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<TestEntity> { WasSuccess = true };
            _mockUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetAsyncById_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<TestEntity> { WasSuccess = false };
            _mockUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            _mockUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task PostAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var entity = new TestEntity();
            var response = new ActionResponse<TestEntity> { WasSuccess = true, Result = entity };
            _mockUnitOfWork.Setup(x => x.AddAsync(entity)).ReturnsAsync(response);

            // Act
            var result = await _controller.PostAsync(entity);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockUnitOfWork.Verify(x => x.AddAsync(entity), Times.Once());
        }

        [TestMethod]
        public async Task PostAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var entity = new TestEntity();
            var response = new ActionResponse<TestEntity> { WasSuccess = false, Message = "Error" };
            _mockUnitOfWork.Setup(x => x.AddAsync(entity)).ReturnsAsync(response);

            // Act
            var result = await _controller.PostAsync(entity);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual(response.Message, badRequestResult!.Value);
            _mockUnitOfWork.Verify(x => x.AddAsync(entity), Times.Once());
        }

        [TestMethod]
        public async Task PutAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var entity = new TestEntity();
            var response = new ActionResponse<TestEntity> { WasSuccess = true, Result = entity };
            _mockUnitOfWork.Setup(x => x.UpdateAsync(entity)).ReturnsAsync(response);

            // Act
            var result = await _controller.PutAsync(entity);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockUnitOfWork.Verify(x => x.UpdateAsync(entity), Times.Once());
        }

        [TestMethod]
        public async Task PutAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var entity = new TestEntity();
            var response = new ActionResponse<TestEntity> { WasSuccess = false, Message = "Error" };
            _mockUnitOfWork.Setup(x => x.UpdateAsync(entity)).ReturnsAsync(response);

            // Act
            var result = await _controller.PutAsync(entity);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual(response.Message, badRequestResult!.Value);
            _mockUnitOfWork.Verify(x => x.UpdateAsync(entity), Times.Once());
        }
    }
}
