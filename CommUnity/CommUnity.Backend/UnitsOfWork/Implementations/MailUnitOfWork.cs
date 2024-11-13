using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class MailUnitOfWork : GenericUnitOfWork<MailArrival>, IMailUnitOfWork
    {
        private readonly IMailRepository _mailRepository;

        public MailUnitOfWork(IGenericRepository<MailArrival> repository, IMailRepository mailRepository) : base(repository)
        {
            _mailRepository = mailRepository;
        }

        public async Task<ActionResponse<MailArrival>> ConfirmMail(string email, MailArrivalDTO mailArrivalDTO) => await _mailRepository.ConfirmMail(email, mailArrivalDTO);

        public async Task<ActionResponse<MailArrival>> UpdateStatusMail(string email, MailArrivalDTO mailArrivalDTO) => await _mailRepository.UpdateStatusMail(email, mailArrivalDTO);

        public async Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByApartment(string email, int apartmentId) => await _mailRepository.GetMailByApartment(email, apartmentId);

        public async Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByStatus(string email, MailStatus status) => await _mailRepository.GetMailByStatus(email, status);

        public async Task<ActionResponse<int>> GetMailRecordsNumber(string email, int id, MailStatus status) => await _mailRepository.GetMailRecordsNumber(email, id, status);

        public async Task<ActionResponse<int>> GetMailRecordsNumberApartment(string email, PaginationMailDTO paginationMailDTO) => await _mailRepository.GetMailRecordsNumberApartment(email, paginationMailDTO);

        public async Task<ActionResponse<int>> GetMailRecordsNumberResidentialUnit(string email, PaginationMailDTO paginationMailDTO) => await _mailRepository.GetMailRecordsNumberResidentialUnit(email, paginationMailDTO);
        
        public async Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByAparmentStatus(string email, PaginationMailDTO paginationMailDTO) => await _mailRepository.GetMailByAparmentStatus(email, paginationMailDTO);

        public async Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByResidentialUnitStatus(string email, PaginationMailDTO paginationMailDTO) => await _mailRepository.GetMailByResidentialUnitStatus(email, paginationMailDTO);

        public async Task<ActionResponse<MailArrival>> RegisterMail(string email, MailArrivalDTO mailArrivalDTO) => await _mailRepository.RegisterMail(email, mailArrivalDTO);
    }
}

