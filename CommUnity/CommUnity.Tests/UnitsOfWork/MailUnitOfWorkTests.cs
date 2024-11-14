using Moq;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using CommUnity.Shared.Enums;

namespace CommUnity.Tests.UnitsOfWork
{
    [TestClass]
    public class MailUnitOfWorkTests
    {
        private Mock<IMailRepository> _mockMailRepository = null!;
        private MailUnitOfWork _unitOfWork = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockMailRepository = new Mock<IMailRepository>();
            var mockGenericRepository = new Mock<IGenericRepository<MailArrival>>();
            _unitOfWork = new MailUnitOfWork(mockGenericRepository.Object, _mockMailRepository.Object);
        }

        [TestMethod]
        public async Task ConfirmMail_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            var mailArrivalDTO = new MailArrivalDTO();
            var expectedResponse = new ActionResponse<MailArrival> { Result = new MailArrival() };
            _mockMailRepository.Setup(x => x.ConfirmMail(email, mailArrivalDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.ConfirmMail(email, mailArrivalDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.ConfirmMail(email, mailArrivalDTO), Times.Once);
        }

        [TestMethod]
        public async Task UpdateStatusMail_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            var mailArrivalDTO = new MailArrivalDTO();
            var expectedResponse = new ActionResponse<MailArrival> { Result = new MailArrival() };
            _mockMailRepository.Setup(x => x.UpdateStatusMail(email, mailArrivalDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.UpdateStatusMail(email, mailArrivalDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.UpdateStatusMail(email, mailArrivalDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetMailByApartment_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            int apartmentId = 1;
            var expectedResponse = new ActionResponse<IEnumerable<MailArrival>> { Result = new List<MailArrival>() };
            _mockMailRepository.Setup(x => x.GetMailByApartment(email, apartmentId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetMailByApartment(email, apartmentId);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.GetMailByApartment(email, apartmentId), Times.Once);
        }

        [TestMethod]
        public async Task GetMailByStatus_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            var status = MailStatus.Delivered;
            var expectedResponse = new ActionResponse<IEnumerable<MailArrival>> { Result = new List<MailArrival>() };
            _mockMailRepository.Setup(x => x.GetMailByStatus(email, status)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetMailByStatus(email, status);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.GetMailByStatus(email, status), Times.Once);
        }

        [TestMethod]
        public async Task GetMailRecordsNumber_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            int id = 1;
            var status = MailStatus.Delivered;
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockMailRepository.Setup(x => x.GetMailRecordsNumber(email, id, status)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetMailRecordsNumber(email, id, status);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.GetMailRecordsNumber(email, id, status), Times.Once);
        }

        [TestMethod]
        public async Task RegisterMail_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            string email = "test@example.com";
            var mailArrivalDTO = new MailArrivalDTO();
            var expectedResponse = new ActionResponse<MailArrival> { Result = new MailArrival() };
            _mockMailRepository.Setup(x => x.RegisterMail(email, mailArrivalDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.RegisterMail(email, mailArrivalDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.RegisterMail(email, mailArrivalDTO), Times.Once);
        }
        [TestMethod]
        public async Task GetMailRecordsNumberApartment_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationMailDTO = new PaginationMailDTO();
            var expectedResponse = new ActionResponse<int> { Result = 10 };
            _mockMailRepository.Setup(x => x.GetMailRecordsNumberApartment(email, paginationMailDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetMailRecordsNumberApartment(email, paginationMailDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.GetMailRecordsNumberApartment(email, paginationMailDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetMailRecordsNumberResidentialUnit_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationMailDTO = new PaginationMailDTO();
            var expectedResponse = new ActionResponse<int> { Result = 20 };
            _mockMailRepository.Setup(x => x.GetMailRecordsNumberResidentialUnit(email, paginationMailDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetMailRecordsNumberResidentialUnit(email, paginationMailDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.GetMailRecordsNumberResidentialUnit(email, paginationMailDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetMailByAparmentStatus_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationMailDTO = new PaginationMailDTO();
            var expectedResponse = new ActionResponse<IEnumerable<MailArrival>> { Result = new List<MailArrival>() };
            _mockMailRepository.Setup(x => x.GetMailByAparmentStatus(email, paginationMailDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetMailByAparmentStatus(email, paginationMailDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.GetMailByAparmentStatus(email, paginationMailDTO), Times.Once);
        }

        [TestMethod]
        public async Task GetMailByResidentialUnitStatus_CallsMailRepositoryAndReturnsResult()
        {
            // Arrange
            var email = "test@example.com";
            var paginationMailDTO = new PaginationMailDTO();
            var expectedResponse = new ActionResponse<IEnumerable<MailArrival>> { Result = new List<MailArrival>() };
            _mockMailRepository.Setup(x => x.GetMailByResidentialUnitStatus(email, paginationMailDTO)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _unitOfWork.GetMailByResidentialUnitStatus(email, paginationMailDTO);

            // Assert
            Assert.AreEqual(expectedResponse, result);
            _mockMailRepository.Verify(x => x.GetMailByResidentialUnitStatus(email, paginationMailDTO), Times.Once);
        }
    }
}
