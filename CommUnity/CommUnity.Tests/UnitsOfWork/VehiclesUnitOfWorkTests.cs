using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class VehiclesUnitOfWorkTests
    {
        private Mock<IGenericRepository<Vehicle>> _mockGenericRepository = null!;
        private Mock<IVehiclesRepository> _mockVehiclesRepository = null!;
        private VehiclesUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericRepository = new Mock<IGenericRepository<Vehicle>>();
            _mockVehiclesRepository = new Mock<IVehiclesRepository>();
            _unitOfWork = new VehiclesUnitOfWork(_mockGenericRepository.Object, _mockVehiclesRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_ById_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            int vehicleId = 1;
            var expectedResponse = new ActionResponse<Vehicle> { Result = new Vehicle { Id = vehicleId, Plate = "ABC123", Type = "Car", Description = "Sedan", ApartmentId = 101 } };
            _mockVehiclesRepository.Setup(x => x.GetAsync(vehicleId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(vehicleId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVehiclesRepository.Verify(x => x.GetAsync(vehicleId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_NoParams_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<Vehicle>> { Result = new List<Vehicle>() };
            _mockVehiclesRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVehiclesRepository.Verify(x => x.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var expectedResponse = new ActionResponse<IEnumerable<Vehicle>> { Result = new List<Vehicle>() };
            _mockVehiclesRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVehiclesRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockVehiclesRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVehiclesRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var expectedResponse = new ActionResponse<int> { Result = 100 };
            _mockVehiclesRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVehiclesRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }
    }
}
