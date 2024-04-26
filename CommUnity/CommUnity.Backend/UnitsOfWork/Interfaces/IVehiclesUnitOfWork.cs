using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Interfaces
{
    public interface IVehiclesUnitOfWork
    {
        Task<ActionResponse<Vehicle>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync();

        Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<Vehicle>>> GetFullAsync(int id);

    }
}
