using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class NewsUnitOfWorkTests
    {
        private Mock<INewsRepository> _mockNewsRepository = null!;
        private NewsUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockNewsRepository = new Mock<INewsRepository>();
            var mockGenericRepository = new Mock<IGenericRepository<News>>();
            _unitOfWork = new NewsUnitOfWork(mockGenericRepository.Object, _mockNewsRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_ById_CallsNewsRepositoryAndReturnsResult()
        {
            // Arrange
            int newsId = 1;
            var expectedResponse = new ActionResponse<News> { Result = new News() };
            _mockNewsRepository.Setup(x => x.GetAsync(newsId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(newsId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockNewsRepository.Verify(x => x.GetAsync(newsId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_NoParams_CallsNewsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<News>> { Result = new List<News>() };
            _mockNewsRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockNewsRepository.Verify(x => x.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsNewsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<News>> { Result = new List<News>() };
            _mockNewsRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockNewsRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsNewsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockNewsRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockNewsRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsNewsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 100 };
            _mockNewsRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockNewsRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }

        [TestMethod]
        public async Task AddFullAsync_CallsNewsRepositoryAndReturnsResult()
        {
            // Arrange
            var newsDTO = new NewsDTO();
            var expectedResponse = new ActionResponse<News> { Result = new News() };
            _mockNewsRepository.Setup(x => x.AddFullAsync(newsDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.AddFullAsync(newsDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockNewsRepository.Verify(x => x.AddFullAsync(newsDTO), Times.Once);
        }

        [TestMethod]
        public async Task UpdateFullAsync_CallsNewsRepositoryAndReturnsResult()
        {
            // Arrange
            var newsDTO = new NewsDTO();
            var expectedResponse = new ActionResponse<News> { Result = new News() };
            _mockNewsRepository.Setup(x => x.UpdateFullAsync(newsDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdateFullAsync(newsDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockNewsRepository.Verify(x => x.UpdateFullAsync(newsDTO), Times.Once);
        }
    }
}
