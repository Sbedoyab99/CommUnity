using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Interfaces
{
    public interface INewsUnitOfWork
    {
        Task<ActionResponse<News>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<News>>> GetAsync();

        Task<ActionResponse<IEnumerable<News>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<News>>> GetFullAsync(int id);

    }
}
