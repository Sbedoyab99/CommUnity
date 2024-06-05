using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface IVisitorEntryRepository
    {
       Task<ActionResponse<VisitorEntry>> ScheduleVisitor(string email, VisitorEntryDTO visitorEntryDTO);
    }
}
