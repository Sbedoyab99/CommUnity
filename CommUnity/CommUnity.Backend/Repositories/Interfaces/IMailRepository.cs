using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface IMailRepository
    {
        Task<ActionResponse<MailArrival>> RegisterMail(string email, MailArrivalDTO mailArrivalDTO);

        Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByStatus(string email, MailStatus status);

        Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByApartment(string email, int apartmentId);

        Task<ActionResponse<int>> GetMailRecordsNumber(string email, int id, MailStatus status);

        Task<ActionResponse<MailArrival>> ConfirmMail(string email, MailArrivalDTO mailArrivalDTO);
    }
}
