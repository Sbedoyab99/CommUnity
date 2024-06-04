using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Interfaces
{
    public interface IPetsUnitOfWork
    {
        Task<ActionResponse<Pet>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Pet>>> GetAsync();

        Task<ActionResponse<Pet>> AddFullAsync(PetDTO petDTO);

        Task<ActionResponse<Pet>> UpdateFullAsync(PetDTO petDTO);

        Task<ActionResponse<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetRecordsNumber(PaginationDTO pagination);
    }
}