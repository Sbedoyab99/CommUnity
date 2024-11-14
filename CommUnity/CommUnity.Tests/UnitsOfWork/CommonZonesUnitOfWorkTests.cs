using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class CommonZonesUnitOfWorkTests
    {
        private Mock<IGenericRepository<CommonZone>> _mockGenericRepository = null!;
        private Mock<ICommonZonesRepository> _mockCommonZonesRepository = null!;
        private CommonZonesUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericRepository = new Mock<IGenericRepository<CommonZone>>();
            _mockCommonZonesRepository = new Mock<ICommonZonesRepository>();
            _unitOfWork = new CommonZonesUnitOfWork(_mockGenericRepository.Object, _mockCommonZonesRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_ById_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            int commonZoneId = 1;
            var expectedResponse = new ActionResponse<CommonZone> { Result = new CommonZone() };
            _mockCommonZonesRepository.Setup(x => x.GetAsync(commonZoneId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(commonZoneId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCommonZonesRepository.Verify(x => x.GetAsync(commonZoneId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_NoParams_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<CommonZone>> { Result = new List<CommonZone>() };
            _mockCommonZonesRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCommonZonesRepository.Verify(x => x.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<CommonZone>> { Result = new List<CommonZone>() };
            _mockCommonZonesRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCommonZonesRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockCommonZonesRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCommonZonesRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockCommonZonesRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCommonZonesRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }
    }
}
