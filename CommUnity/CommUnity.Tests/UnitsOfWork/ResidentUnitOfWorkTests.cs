using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class ResidentUnitOfWorkTests
    {
        private Mock<IResidentRepository> _mockResidentRepository = null!;
        private ResidentUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockResidentRepository = new Mock<IResidentRepository>();
            var mockGenericRepository = new Mock<IGenericRepository<User>>();
            _unitOfWork = new ResidentUnitOfWork(mockGenericRepository.Object, _mockResidentRepository.Object);
        }

        [TestMethod]
        public async Task GetResidentAsync_CallsResidentRepositoryAndReturnsResult()
        {
            // Arrange
            int apartmentId = 1;
            var expectedResponse = new ActionResponse<IEnumerable<User>> { Result = new List<User>() };
            _mockResidentRepository.Setup(x => x.GetResidentAsync(apartmentId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetResidentAsync(apartmentId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockResidentRepository.Verify(x => x.GetResidentAsync(apartmentId), Times.Once);
        }

        [TestMethod]
        public async Task GetResidentAsync_NoResidents_ReturnsEmptyList()
        {
            // Arrange
            int apartmentId = 1;
            var expectedResponse = new ActionResponse<IEnumerable<User>> { Result = new List<User>() };
            _mockResidentRepository.Setup(x => x.GetResidentAsync(apartmentId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetResidentAsync(apartmentId);

            // Assert
            Assert.IsTrue(result.Result.Count() == 0); // Expecting empty list
            _mockResidentRepository.Verify(x => x.GetResidentAsync(apartmentId), Times.Once);
        }

        [TestMethod]
        public async Task GetResidentAsync_WithResidents_ReturnsList()
        {
            // Arrange
            int apartmentId = 1;
            var expectedResponse = new ActionResponse<IEnumerable<User>> { Result = new List<User> { new User {} } };
            _mockResidentRepository.Setup(x => x.GetResidentAsync(apartmentId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetResidentAsync(apartmentId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            Assert.AreEqual(1, result.Result.Count()); // Expecting 1 resident
            _mockResidentRepository.Verify(x => x.GetResidentAsync(apartmentId), Times.Once);
        }
    }
}
