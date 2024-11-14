using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class PetsUnitOfWorkTests
    {
        private Mock<IPetsRepository> _mockPetsRepository = null!;
        private PetsUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockPetsRepository = new Mock<IPetsRepository>();
            var mockGenericRepository = new Mock<IGenericRepository<Pet>>();
            _unitOfWork = new PetsUnitOfWork(mockGenericRepository.Object, _mockPetsRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_CallsPetsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<Pet>> { Result = new List<Pet>() };
            _mockPetsRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPetsRepository.Verify(x => x.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_ById_CallsPetsRepositoryAndReturnsResult()
        {
            // Arrange
            int petId = 1;
            var expectedResponse = new ActionResponse<Pet> { Result = new Pet() };
            _mockPetsRepository.Setup(x => x.GetAsync(petId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(petId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPetsRepository.Verify(x => x.GetAsync(petId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsPetsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<Pet>> { Result = new List<Pet>() };
            _mockPetsRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPetsRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsPetsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockPetsRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPetsRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsPetsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockPetsRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPetsRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }

        [TestMethod]
        public async Task AddFullAsync_CallsPetsRepositoryAndReturnsResult()
        {
            // Arrange
            var petDTO = new PetDTO();
            var expectedResponse = new ActionResponse<Pet> { Result = new Pet() };
            _mockPetsRepository.Setup(x => x.AddFullAsync(petDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.AddFullAsync(petDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPetsRepository.Verify(x => x.AddFullAsync(petDTO), Times.Once);
        }

        [TestMethod]
        public async Task UpdateFullAsync_CallsPetsRepositoryAndReturnsResult()
        {
            // Arrange
            var petDTO = new PetDTO();
            var expectedResponse = new ActionResponse<Pet> { Result = new Pet() };
            _mockPetsRepository.Setup(x => x.UpdateFullAsync(petDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdateFullAsync(petDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPetsRepository.Verify(x => x.UpdateFullAsync(petDTO), Times.Once);
        }
    }
}
