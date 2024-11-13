using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Interfaces
{
    public interface IMailUnitOfWork
    {
        Task<ActionResponse<MailArrival>> RegisterMail(string email, MailArrivalDTO mailArrivalDTO);

        Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByStatus(string email, MailStatus status);

        Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByApartment(string email, int apartmentId);

        Task<ActionResponse<int>> GetMailRecordsNumber(string email, int id, MailStatus status);

        Task<ActionResponse<int>> GetMailRecordsNumberApartment(string email, PaginationMailDTO paginationMailDTO);

        Task<ActionResponse<int>> GetMailRecordsNumberResidentialUnit(string email, PaginationMailDTO paginationMailDTO);

        Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByAparmentStatus(string email, PaginationMailDTO paginationMailDTO);

        Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByResidentialUnitStatus(string email, PaginationMailDTO paginationMailDTO);

        Task<ActionResponse<MailArrival>> ConfirmMail(string email, MailArrivalDTO mailArrivalDTO);

        Task<ActionResponse<MailArrival>> UpdateStatusMail(string email, MailArrivalDTO mailArrivalDTO);
    }
}
