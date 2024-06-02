using Microsoft.AspNetCore.Mvc;
using Moq;
using CommUnity.BackEnd.Controllers;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.BackEnd.Helpers;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommUnity.Tests.Controllers
{
    [TestClass]
    public class PetsControllerTests
    {
        private Mock<IGenericUnitOfWork<Pet>> _mockGenericUnitOfWork = null!;
        private Mock<IPetsUnitOfWork> _mockPetsUnitOfWork = null!;
        private Mock<IFileStorage> _mockFileStorage = null!;
        private PetsController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericUnitOfWork = new Mock<IGenericUnitOfWork<Pet>>();
            _mockPetsUnitOfWork = new Mock<IPetsUnitOfWork>();
            _mockFileStorage = new Mock<IFileStorage>();
            _controller = new PetsController(_mockGenericUnitOfWork.Object, _mockPetsUnitOfWork.Object, _mockFileStorage.Object);
        }

        [TestMethod]
        public async Task GetAsync_Full_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<Pet>> { WasSuccess = true, Result = new List<Pet>() };
            _mockPetsUnitOfWork.Setup(x => x.GetAsync()).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_Full_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var response = new ActionResponse<IEnumerable<Pet>> { WasSuccess = false };
            _mockPetsUnitOfWork.Setup(x => x.GetAsync()).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockPetsUnitOfWork.Verify(x => x.GetAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<Pet>> { WasSuccess = true, Result = new List<Pet>() };
            _mockPetsUnitOfWork.Setup(x => x.GetAsync(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_WithPagination_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<IEnumerable<Pet>> { WasSuccess = false };
            _mockPetsUnitOfWork.Setup(x => x.GetAsync(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockPetsUnitOfWork.Verify(x => x.GetAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<Pet> { WasSuccess = true, Result = new Pet() };
            _mockPetsUnitOfWork.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetAsync_ById_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var id = 1;
            var response = new ActionResponse<Pet> { WasSuccess = false, Message = "Not Found" };
            _mockPetsUnitOfWork.Setup(x => x.GetAsync(id)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(response.Message, notFoundResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.GetAsync(id), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = true, Result = 5 };
            _mockPetsUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetPagesAsync_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var action = new ActionResponse<int> { WasSuccess = false };
            _mockPetsUnitOfWork.Setup(x => x.GetTotalPagesAsync(pagination)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.GetPagesAsync(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockPetsUnitOfWork.Verify(x => x.GetTotalPagesAsync(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = true, Result = 10 };
            _mockPetsUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(response.Result, okResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }

        [TestMethod]
        public async Task GetRecordsNumber_ReturnsBadRequestResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var pagination = new PaginationDTO();
            var response = new ActionResponse<int> { WasSuccess = false };
            _mockPetsUnitOfWork.Setup(x => x.GetRecordsNumber(pagination)).Returns(Task.FromResult(response));

            // Act
            var result = await _controller.GetRecordsNumber(pagination);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockPetsUnitOfWork.Verify(x => x.GetRecordsNumber(pagination), Times.Once());
        }

        [TestMethod]
        public async Task PostFullAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var petDTO = new PetDTO();
            var action = new ActionResponse<Pet> { WasSuccess = true, Result = new Pet() };
            _mockPetsUnitOfWork.Setup(x => x.AddFullAsync(petDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PostFullAsync(petDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.AddFullAsync(petDTO), Times.Once());
        }

        [TestMethod]
        public async Task PostFullAsync_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var petDTO = new PetDTO();
            var action = new ActionResponse<Pet> { WasSuccess = false, Message = "Error" };
            _mockPetsUnitOfWork.Setup(x => x.AddFullAsync(petDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PostFullAsync(petDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(action.Message, notFoundResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.AddFullAsync(petDTO), Times.Once());
        }

        [TestMethod]
        public async Task PutFullAsync_ReturnsOkObjectResult_WhenWasSuccessIsTrue()
        {
            // Arrange
            var petDTO = new PetDTO();
            var action = new ActionResponse<Pet> { WasSuccess = true, Result = new Pet() };
            _mockPetsUnitOfWork.Setup(x => x.UpdateFullAsync(petDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PutFullAsync(petDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(action.Result, okResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.UpdateFullAsync(petDTO), Times.Once());
        }

        [TestMethod]
        public async Task PutFullAsync_ReturnsNotFoundResult_WhenWasSuccessIsFalse()
        {
            // Arrange
            var petDTO = new PetDTO();
            var action = new ActionResponse<Pet> { WasSuccess = false, Message = "Error" };
            _mockPetsUnitOfWork.Setup(x => x.UpdateFullAsync(petDTO)).Returns(Task.FromResult(action));

            // Act
            var result = await _controller.PutFullAsync(petDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(action.Message, notFoundResult!.Value);
            _mockPetsUnitOfWork.Verify(x => x.UpdateFullAsync(petDTO), Times.Once());
        }

    }
}