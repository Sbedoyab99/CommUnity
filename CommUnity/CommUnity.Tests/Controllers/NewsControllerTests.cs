using CommUnity.BackEnd.Controllers;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CommUnity.Tests.Controllers
{
    [TestClass]
    public class NewsControllerTests
    {
        private Mock<IGenericUnitOfWork<News>> _mockGenericUnitOfWork;
        private Mock<INewsUnitOfWork> _mockNewsUnitOfWork;
        private Mock<IFileStorage> _mockFileStorage;
        private NewsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericUnitOfWork = new Mock<IGenericUnitOfWork<News>>();
            _mockNewsUnitOfWork = new Mock<INewsUnitOfWork>();
            _mockFileStorage = new Mock<IFileStorage>();
            _controller = new NewsController(_mockGenericUnitOfWork.Object, _mockNewsUnitOfWork.Object, _mockFileStorage.Object);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var newsList = new List<News> { new News(), new News() };
            var response = new ActionResponse<IEnumerable<News>> { WasSuccess = true, Result = newsList };
            _mockNewsUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(newsList, okResult!.Value);
        }

        [TestMethod]
        public async Task GetAsync_ReturnsBadRequest_WhenWasSuccessIsFalse()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<News>> { WasSuccess = false };
            _mockNewsUnitOfWork.Setup(x => x.GetAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var newsList = new List<News> { new News(), new News() };
            var response = new ActionResponse<IEnumerable<News>> { WasSuccess = true, Result = newsList };
            _mockNewsUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(newsList, okResult!.Value);
        }

        [TestMethod]
        public async Task GetAsync_Pagination_ReturnsBadRequest_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<News>> { WasSuccess = false };
            _mockNewsUnitOfWork.Setup(x => x.GetAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var totalPages = 5;
            var response = new ActionResponse<int> { WasSuccess = true, Result = totalPages };
            _mockNewsUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(totalPages, okResult!.Value);
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsBadRequest_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockNewsUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetAsync_Id_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var news = new News();
            var response = new ActionResponse<News> { WasSuccess = true, Result = news };
            _mockNewsUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(news, okResult!.Value);
        }

        [TestMethod]
        public async Task GetAsync_Id_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<News> { WasSuccess = false, Message = "Not found" };
            _mockNewsUnitOfWork.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(response.Message, notFoundResult!.Value);
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var recordNumber = 100;
            var response = new ActionResponse<int> { WasSuccess = true, Result = recordNumber };
            _mockNewsUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(recordNumber, okResult!.Value);
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsBadRequest_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockNewsUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task PostFullAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var newsDTO = new NewsDTO();
            var action = new ActionResponse<News> { WasSuccess = true, Result = new News() };
            _mockNewsUnitOfWork.Setup(x => x.AddFullAsync(newsDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PostFullAsync(newsDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockNewsUnitOfWork.Verify(x => x.AddFullAsync(newsDTO), Times.Once());
        }

        [TestMethod]
        public async Task PostFullAsync_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var newsDTO = new NewsDTO();
            var action = new ActionResponse<News> { WasSuccess = false, Message = "Error" };
            _mockNewsUnitOfWork.Setup(x => x.AddFullAsync(newsDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PostFullAsync(newsDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(action.Message, notFoundResult!.Value);
            _mockNewsUnitOfWork.Verify(x => x.AddFullAsync(newsDTO), Times.Once());
        }

        [TestMethod]
        public async Task PutFullAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var newsDTO = new NewsDTO();
            var action = new ActionResponse<News> { WasSuccess = true, Result = new News() };
            _mockNewsUnitOfWork.Setup(x => x.UpdateFullAsync(newsDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PutFullAsync(newsDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockNewsUnitOfWork.Verify(x => x.UpdateFullAsync(newsDTO), Times.Once());
        }

        [TestMethod]
        public async Task PutFullAsync_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var newsDTO = new NewsDTO();
            var action = new ActionResponse<News> { WasSuccess = false, Message = "Error" };
            _mockNewsUnitOfWork.Setup(x => x.UpdateFullAsync(newsDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PutFullAsync(newsDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(action.Message, notFoundResult!.Value);
            _mockNewsUnitOfWork.Verify(x => x.UpdateFullAsync(newsDTO), Times.Once());
        }

    }
}