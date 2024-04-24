using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface ICommonZonesRepository
    {
        Task<ActionResponse<CommonZone>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<CommonZone>>> GetAsync();

        Task<ActionResponse<IEnumerable<CommonZone>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<CommonZone>>> GetFullAsync(int id);
    }
}
