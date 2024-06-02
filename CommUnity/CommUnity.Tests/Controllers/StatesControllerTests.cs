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
    public class StatesControllerTests
    {
        private Mock<IGenericUnitOfWork<State>> _mockGenericUnitOfWork = null!;
        private Mock<IStatesUnitOfWork> _mockStatesUnitOfWork = null!;
        private StatesController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericUnitOfWork = new Mock<IGenericUnitOfWork<State>>();
            _mockStatesUnitOfWork = new Mock<IStatesUnitOfWork>();
            _controller = new StatesController(_mockGenericUnitOfWork.Object, _mockStatesUnitOfWork.Object);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<State>> { WasSuccess = true, Result = new List<State>() };
            _mockStatesUnitOfWork.Setup(x => x.GetAsync(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockStatesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<State>> { WasSuccess = false };
            _mockStatesUnitOfWork.Setup(x => x.GetAsync(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockStatesUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = true, Result = 5 };
            _mockStatesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockStatesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = false };
            _mockStatesUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockStatesUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<State> { WasSuccess = true, Result = new State() };
            _mockStatesUnitOfWork.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockStatesUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<State> { WasSuccess = false, Message = "Not Found" };
            _mockStatesUnitOfWork.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(response.Message, notFoundResult!.Value);
            _mockStatesUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetComboAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var countryId = 1;
            var comboList = new List<State>();
            _mockStatesUnitOfWork.Setup(x => x.GetComboAsync(countryId)).Returns(Task.FromResult<IEnumerable<State>>(comboList));

            // Act
            var result = await _controller.GetComboAsync(countryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(comboList, okResult!.Value);
            _mockStatesUnitOfWork.Verify(x => x.GetComboAsync(countryId), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = true, Result = 10 };
            _mockStatesUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockStatesUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockStatesUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockStatesUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }
    }
}
