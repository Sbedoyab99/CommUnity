using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface IPqrsRepository
    {

        Task<ActionResponse<Pqrs>> CreatePqrs(string email, PqrsDTO pqrsDTO);

        Task<ActionResponse<IEnumerable<Pqrs>>> GetPqrsByTypeByStatus(string email, PqrsType type, PqrsState status);

        Task<ActionResponse<int>> GetPqrsRecordsNumber(string email, int id, PqrsType type, PqrsState status);

    }
}
