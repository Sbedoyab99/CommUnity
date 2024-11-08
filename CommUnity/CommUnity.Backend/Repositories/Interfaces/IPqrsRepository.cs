using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface IPqrsRepository
    {

        Task<ActionResponse<Pqrs>> GetAsync(int id);
        Task<ActionResponse<Pqrs>> CreatePqrs(string email, PqrsDTO pqrsDTO);


        Task<ActionResponse<IEnumerable<Pqrs>>> GetPqrsByTypeByStatus(string email, PaginationPqrsDTO paginationPqrs);

        Task<ActionResponse<int>> GetPqrsRecordsNumber(string email, PaginationPqrsDTO paginationPqrs);

        Task<ActionResponse<Pqrs>> UpdatePqrs(PqrsDTO pqrsDTO);

        Task<ActionResponse<Pqrs>> UpdateStatusPqrs(PqrsDTO pqrsDTO);

    }
}
