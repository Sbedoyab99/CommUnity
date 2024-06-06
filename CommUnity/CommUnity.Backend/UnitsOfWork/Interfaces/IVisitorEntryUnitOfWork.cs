using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Interfaces
{
    public interface IVisitorEntryUnitOfWork
    {
        Task<ActionResponse<VisitorEntry>> ScheduleVisitor(string email, VisitorEntryDTO visitorEntryDTO);

        Task<ActionResponse<IEnumerable<VisitorEntry>>> GetVisitorEntryByStatus(string email, VisitorStatus status);

        Task<ActionResponse<VisitorEntry>> ConfirmVisitorEntry(string email, VisitorEntryDTO visitorEntryDTO);
    }
}
