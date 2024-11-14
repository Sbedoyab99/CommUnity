using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class StatesUnitOfWorkTests
    {
        private Mock<IStatesRepository> _mockStatesRepository = null!;
        private StatesUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockStatesRepository = new Mock<IStatesRepository>();
            var mockGenericRepository = new Mock<IGenericRepository<State>>();
            _unitOfWork = new StatesUnitOfWork(mockGenericRepository.Object, _mockStatesRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_CallsStatesRepositoryAndReturnsResult()
        {
            // Arrange
            int stateId = 1;
            var expectedResponse = new ActionResponse<State> { Result = new State { Id = stateId, Name = "California" } };
            _mockStatesRepository.Setup(x => x.GetAsync(stateId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(stateId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockStatesRepository.Verify(x => x.GetAsync(stateId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsPagedResults()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var expectedResponse = new ActionResponse<IEnumerable<State>> { Result = new List<State> { new State { Id = 1, Name = "California" } } };
            _mockStatesRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockStatesRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsStatesRepositoryAndReturnsTotalPages()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockStatesRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockStatesRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetComboAsync_CallsStatesRepositoryAndReturnsStateList()
        {
            // Arrange
            int countryId = 1;
            var expectedResponse = new List<State> { new State { Id = 1, Name = "California" } };
            _mockStatesRepository.Setup(x => x.GetComboAsync(countryId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetComboAsync(countryId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockStatesRepository.Verify(x => x.GetComboAsync(countryId), Times.Once);
        }

        [TestMethod]
        public async Task GetRecordsNumber_CallsStatesRepositoryAndReturnsRecordCount()
        {
            // Arrange
            var pagination = new PaginationDTO { Page = 1, RecordsNumber = 10 };
            var expectedResponse = new ActionResponse<int> { Result = 100 };
            _mockStatesRepository.Setup(x => x.GetRecordsNumber(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetRecordsNumber(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockStatesRepository.Verify(x => x.GetRecordsNumber(pagination), Times.Once);
        }
    }
}
