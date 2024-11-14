using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class ResidentialUnitUnitOfWorkTests
    {
        private Mock<IResidentialUnitRepository> _mockResidentialUnitRepository = null!;
        private ResidentialUnitUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockResidentialUnitRepository = new Mock<IResidentialUnitRepository>();
            var mockGenericRepository = new Mock<IGenericRepository<ResidentialUnit>>();
            _unitOfWork = new ResidentialUnitUnitOfWork(mockGenericRepository.Object, _mockResidentialUnitRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_CallsResidentialUnitRepositoryAndReturnsResult()
        {
            // Arrange
            int residentialUnitId = 1;
            var expectedResponse = new ActionResponse<ResidentialUnit> { Result = new ResidentialUnit() };
            _mockResidentialUnitRepository.Setup(x => x.GetAsync(residentialUnitId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(residentialUnitId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockResidentialUnitRepository.Verify(x => x.GetAsync(residentialUnitId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsResidentialUnitRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<ResidentialUnit>> { Result = new List<ResidentialUnit>() };
            _mockResidentialUnitRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockResidentialUnitRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsResidentialUnitRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockResidentialUnitRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockResidentialUnitRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsResidentialUnitRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 100 };
            _mockResidentialUnitRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockResidentialUnitRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_NoPagination_CallsResidentialUnitRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<ResidentialUnit>> { Result = new List<ResidentialUnit>() };
            _mockResidentialUnitRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockResidentialUnitRepository.Verify(x => x.GetAsync(), Times.Once);
        }
    }
}
