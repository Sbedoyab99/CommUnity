using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface IVehiclesRepository
    {
        Task<ActionResponse<Vehicle>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync();

        Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<Vehicle>>> GetFullAsync(int id);
    }
}
