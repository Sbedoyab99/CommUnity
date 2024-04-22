using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Interfaces
{
    public interface IResidentialUnitUnitOfWork
    {
        Task<ActionResponse<ResidentialUnit>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<ResidentialUnit>>> GetAsync();

        Task<ActionResponse<IEnumerable<ResidentialUnit>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<ResidentialUnit>>> GetFullAsync(int id);
    }
}