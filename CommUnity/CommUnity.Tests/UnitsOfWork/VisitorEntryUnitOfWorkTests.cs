using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class VisitorEntryUnitOfWorkTests
    {
        private Mock<IGenericRepository<VisitorEntry>> _mockGenericRepository = null!;
        private Mock<IVisitorEntryRepository> _mockVisitorEntryRepository = null!;
        private VisitorEntryUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockGenericRepository = new Mock<IGenericRepository<VisitorEntry>>();
            _mockVisitorEntryRepository = new Mock<IVisitorEntryRepository>();
            _unitOfWork = new VisitorEntryUnitOfWork(_mockGenericRepository.Object, _mockVisitorEntryRepository.Object);
        }

        [TestMethod]
        public async Task GetVisitorEntryByStatus_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var status = VisitorStatus.Approved;
            var expectedResponse = new ActionResponse<IEnumerable<VisitorEntry>> { Result = new List<VisitorEntry>() };
            _mockVisitorEntryRepository.Setup(x => x.GetVisitorEntryByStatus(email, status)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetVisitorEntryByStatus(email, status);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.GetVisitorEntryByStatus(email, status), Times.Once);
        }

        [TestMethod]
        public async Task ScheduleVisitor_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var visitorEntryDTO = new VisitorEntryDTO();
            var expectedResponse = new ActionResponse<VisitorEntry> { Result = new VisitorEntry() };
            _mockVisitorEntryRepository.Setup(x => x.ScheduleVisitor(email, visitorEntryDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.ScheduleVisitor(email, visitorEntryDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.ScheduleVisitor(email, visitorEntryDTO), Times.Once);
        }

        [TestMethod]
        public async Task AddVisitor_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var visitorEntryDTO = new VisitorEntryDTO();
            var expectedResponse = new ActionResponse<VisitorEntry> { Result = new VisitorEntry() };
            _mockVisitorEntryRepository.Setup(x => x.AddVisitor(email, visitorEntryDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.AddVisitor(email, visitorEntryDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.AddVisitor(email, visitorEntryDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetVisitorEntryByApartment_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var apartmentId = 101;
            var expectedResponse = new ActionResponse<IEnumerable<VisitorEntry>> { Result = new List<VisitorEntry>() };
            _mockVisitorEntryRepository.Setup(x => x.GetVisitorEntryByApartment(email, apartmentId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetVisitorEntryByApartment(email, apartmentId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.GetVisitorEntryByApartment(email, apartmentId), Times.Once);
        }

        [TestMethod]
        public async Task GetVisitorEntryRecordsNumber_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var id = 1;
            var status = VisitorStatus.Approved;
            var expectedResponse = new ActionResponse<int> { Result = 5 };
            _mockVisitorEntryRepository.Setup(x => x.GetVisitorEntryRecordsNumber(email, id, status)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetVisitorEntryRecordsNumber(email, id, status);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.GetVisitorEntryRecordsNumber(email, id, status), Times.Once);
        }

        [TestMethod]
        public async Task GetVisitorRecordsNumberApartment_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationVisitorDTO = new PaginationVisitorDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockVisitorEntryRepository.Setup(x => x.GetVisitorRecordsNumberApartment(email, paginationVisitorDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetVisitorRecordsNumberApartment(email, paginationVisitorDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.GetVisitorRecordsNumberApartment(email, paginationVisitorDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetVisitorRecordsNumberResidentialUnit_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationVisitorDTO = new PaginationVisitorDTO();
            var expectedResponse = new ActionResponse<int> { Result = 15 };
            _mockVisitorEntryRepository.Setup(x => x.GetVisitorRecordsNumberResidentialUnit(email, paginationVisitorDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetVisitorRecordsNumberResidentialUnit(email, paginationVisitorDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.GetVisitorRecordsNumberResidentialUnit(email, paginationVisitorDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetVisitorEntryByAparmentStatus_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationVisitorDTO = new PaginationVisitorDTO();
            var expectedResponse = new ActionResponse<IEnumerable<VisitorEntry>> { Result = new List<VisitorEntry>() };
            _mockVisitorEntryRepository.Setup(x => x.GetVisitorEntryByAparmentStatus(email, paginationVisitorDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetVisitorEntryByAparmentStatus(email, paginationVisitorDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.GetVisitorEntryByAparmentStatus(email, paginationVisitorDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetVisitorByResidentialUnitStatus_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationVisitorDTO = new PaginationVisitorDTO();
            var expectedResponse = new ActionResponse<IEnumerable<VisitorEntry>> { Result = new List<VisitorEntry>() };
            _mockVisitorEntryRepository.Setup(x => x.GetVisitorByResidentialUnitStatus(email, paginationVisitorDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetVisitorByResidentialUnitStatus(email, paginationVisitorDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.GetVisitorByResidentialUnitStatus(email, paginationVisitorDTO), Times.Once);
        }

        [TestMethod]
        public async Task ConfirmVisitorEntry_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var visitorEntryDTO = new VisitorEntryDTO();
            var expectedResponse = new ActionResponse<VisitorEntry> { Result = new VisitorEntry() };
            _mockVisitorEntryRepository.Setup(x => x.ConfirmVisitorEntry(email, visitorEntryDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.ConfirmVisitorEntry(email, visitorEntryDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.ConfirmVisitorEntry(email, visitorEntryDTO), Times.Once);
        }

        [TestMethod]
        public async Task CancelVisitorEntry_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var visitorEntryDTO = new VisitorEntryDTO();
            var expectedResponse = new ActionResponse<VisitorEntry> { Result = new VisitorEntry() };
            _mockVisitorEntryRepository.Setup(x => x.CancelVisitorEntry(email, visitorEntryDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.CancelVisitorEntry(email, visitorEntryDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockVisitorEntryRepository.Verify(x => x.CancelVisitorEntry(email, visitorEntryDTO), Times.Once);
        }

    }
}
