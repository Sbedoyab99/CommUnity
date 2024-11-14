using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class GenericUnitOfWorkTests
    {
        private Mock<IGenericRepository<TestEntity>> _mockRepository = null!;
        private GenericUnitOfWork<TestEntity> _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IGenericRepository<TestEntity>>();
            _unitOfWork = new GenericUnitOfWork<TestEntity>(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetAsync_ById_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            int entityId = 1;
            var expectedResponse = new ActionResponse<TestEntity> { Result = new TestEntity() };
            _mockRepository.Setup(x => x.GetAsync(entityId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(entityId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockRepository.Verify(x => x.GetAsync(entityId), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_NoParams_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var expectedResponse = new ActionResponse<IEnumerable<TestEntity>> { Result = new List<TestEntity>() };
            _mockRepository.Setup(x => x.GetAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync();

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockRepository.Verify(x => x.GetAsync(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<IEnumerable<TestEntity>> { Result = new List<TestEntity>() };
            _mockRepository.Setup(x => x.GetAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockRepository.Verify(x => x.GetAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task GetTotalPagesAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockRepository.Setup(x => x.GetTotalPagesAsync(pagination)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetTotalPagesAsync(pagination);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockRepository.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once);
        }

        [TestMethod]
        public async Task AddAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var entity = new TestEntity();
            var expectedResponse = new ActionResponse<TestEntity> { Result = entity };
            _mockRepository.Setup(x => x.AddAsync(entity)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.AddAsync(entity);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockRepository.Verify(x => x.AddAsync(entity), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            int entityId = 1;
            var expectedResponse = new ActionResponse<TestEntity> { Result = new TestEntity() };
            _mockRepository.Setup(x => x.DeleteAsync(entityId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.DeleteAsync(entityId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockRepository.Verify(x => x.DeleteAsync(entityId), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var entity = new TestEntity();
            var expectedResponse = new ActionResponse<TestEntity> { Result = entity };
            _mockRepository.Setup(x => x.UpdateAsync(entity)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdateAsync(entity);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockRepository.Verify(x => x.UpdateAsync(entity), Times.Once);
        }
    }

    public class TestEntity { }  // Dummy class for testing purposes
}
