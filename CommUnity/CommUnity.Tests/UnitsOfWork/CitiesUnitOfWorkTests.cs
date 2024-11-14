using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class CitiesUnitOfWorkTests
    {
        private Mock<IGenericRepository<City>> _mockGenericRepository = null!;
        private Mock<ICitiesRepository> _mockCitiesRepository = null!;
        private CitiesUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericRepository = new Mock<IGenericRepository<City>>();
            _mockCitiesRepository = new Mock<ICitiesRepository>();
            _unitOfWork = new CitiesUnitOfWork(_mockGenericRepository.Object, _mockCitiesRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<City>> { Result = new List<City>() };
            _mockCitiesRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCitiesRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockCitiesRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCitiesRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetComboAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            int stateId = 1;
            var expectedCities = new List<City> { new City() };
            _mockCitiesRepository.Setup(x => x.GetComboAsync(stateId)).ReturnsAsync(expectedCities);

            // Act
            var result = await _unitOfWork.GetComboAsync(stateId);

            // Assert
            Assert.AreEqual(expectedCities, result);
            _mockCitiesRepository.Verify(x => x.GetComboAsync(stateId), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockCitiesRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCitiesRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }
    }
}
