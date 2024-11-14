using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class PqrsUnitOfWorkTests
    {
        private Mock<IPqrsRepository> _mockPqrsRepository = null!;
        private PqrsUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockPqrsRepository = new Mock<IPqrsRepository>();
            var mockGenericRepository = new Mock<IGenericRepository<Pqrs>>();
            _unitOfWork = new PqrsUnitOfWork(mockGenericRepository.Object, _mockPqrsRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_CallsPqrsRepositoryAndReturnsResult()
        {
            // Arrange
            int pqrsId = 1;
            var expectedResponse = new ActionResponse<Pqrs> { Result = new Pqrs() };
            _mockPqrsRepository.Setup(x => x.GetAsync(pqrsId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pqrsId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPqrsRepository.Verify(x => x.GetAsync(pqrsId), Times.Once);
        }

        [TestMethod]
        public async Task CreatePqrs_CallsPqrsRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            var pqrsDTO = new PqrsDTO();
            var expectedResponse = new ActionResponse<Pqrs> { Result = new Pqrs() };
            _mockPqrsRepository.Setup(x => x.CreatePqrs(email, pqrsDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.CreatePqrs(email, pqrsDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPqrsRepository.Verify(x => x.CreatePqrs(email, pqrsDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetPqrsByTypeByStatus_CallsPqrsRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            var paginationPqrs = new PaginationPqrsDTO();
            var expectedResponse = new ActionResponse<IEnumerable<Pqrs>> { Result = new List<Pqrs>() };
            _mockPqrsRepository.Setup(x => x.GetPqrsByTypeByStatus(email, paginationPqrs)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetPqrsByTypeByStatus(email, paginationPqrs);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPqrsRepository.Verify(x => x.GetPqrsByTypeByStatus(email, paginationPqrs), Times.Once);
        }

        [TestMethod]
        public async Task GetPqrsRecordsNumber_CallsPqrsRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            var paginationPqrs = new PaginationPqrsDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockPqrsRepository.Setup(x => x.GetPqrsRecordsNumber(email, paginationPqrs)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetPqrsRecordsNumber(email, paginationPqrs);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPqrsRepository.Verify(x => x.GetPqrsRecordsNumber(email, paginationPqrs), Times.Once);
        }

        [TestMethod]
        public async Task UpdatePqrs_CallsPqrsRepositoryAndReturnsResult()
        {
            // Arrange
            var pqrsDTO = new PqrsDTO();
            var expectedResponse = new ActionResponse<Pqrs> { Result = new Pqrs() };
            _mockPqrsRepository.Setup(x => x.UpdatePqrs(pqrsDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdatePqrs(pqrsDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPqrsRepository.Verify(x => x.UpdatePqrs(pqrsDTO), Times.Once);
        }

        [TestMethod]
        public async Task UpdateStatusPqrs_CallsPqrsRepositoryAndReturnsResult()
        {
            // Arrange
            var pqrsDTO = new PqrsDTO();
            var expectedResponse = new ActionResponse<Pqrs> { Result = new Pqrs() };
            _mockPqrsRepository.Setup(x => x.UpdateStatusPqrs(pqrsDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdateStatusPqrs(pqrsDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockPqrsRepository.Verify(x => x.UpdateStatusPqrs(pqrsDTO), Times.Once);
        }
    }
}
