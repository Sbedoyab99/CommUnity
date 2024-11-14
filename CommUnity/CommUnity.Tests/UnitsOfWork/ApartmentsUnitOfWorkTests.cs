using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class ApartmentsUnitOfWorkTests
    {
        private Mock<IGenericRepository<Apartment>> _mockGenericRepository = null!;
        private Mock<IApartmentsRepository> _mockApartmentsRepository = null!;
        private ApartmentsUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericRepository = new Mock<IGenericRepository<Apartment>>();
            _mockApartmentsRepository = new Mock<IApartmentsRepository>();
            _unitOfWork = new ApartmentsUnitOfWork(_mockGenericRepository.Object, _mockApartmentsRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_ById_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            int apartmentId = 1;
            var expectedResponse = new ActionResponse<Apartment> { Result = new Apartment() };
            _mockApartmentsRepository.Setup(x => x.GetAsync(apartmentId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(apartmentId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockApartmentsRepository.Verify(x => x.GetAsync(apartmentId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_NoParams_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<Apartment>> { Result = new List<Apartment>() };
            _mockApartmentsRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockApartmentsRepository.Verify(x => x.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<Apartment>> { Result = new List<Apartment>() };
            _mockApartmentsRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockApartmentsRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockApartmentsRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockApartmentsRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockApartmentsRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockApartmentsRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }
    }
}
