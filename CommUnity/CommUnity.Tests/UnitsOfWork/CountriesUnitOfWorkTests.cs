using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class CountriesUnitOfWorkTests
    {
        private Mock<IGenericRepository<Country>> _mockGenericRepository = null!;
        private Mock<ICountriesRepository> _mockCountriesRepository = null!;
        private CountriesUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericRepository = new Mock<IGenericRepository<Country>>();
            _mockCountriesRepository = new Mock<ICountriesRepository>();
            _unitOfWork = new CountriesUnitOfWork(_mockGenericRepository.Object, _mockCountriesRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_ById_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            int countryId = 1;
            var expectedResponse = new ActionResponse<Country> { Result = new Country() };
            _mockCountriesRepository.Setup(x => x.GetAsync(countryId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(countryId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCountriesRepository.Verify(x => x.GetAsync(countryId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_NoParams_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<Country>> { Result = new List<Country>() };
            _mockCountriesRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCountriesRepository.Verify(x => x.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<Country>> { Result = new List<Country>() };
            _mockCountriesRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCountriesRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockCountriesRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCountriesRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetComboAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedCountries = new List<Country> { new Country() };
            _mockCountriesRepository.Setup(x => x.GetComboAsync()).ReturnsAsync(expectedCountries);

            // Act
            var result = await _unitOfWork.GetComboAsync();

            // Assert
            Assert.AreEqual(expectedCountries, result);
            _mockCountriesRepository.Verify(x => x.GetComboAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockCountriesRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockCountriesRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }
    }
}
