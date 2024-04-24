using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Interfaces
{
    public interface INewsRepository
    {
        Task<ActionResponse<News>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<News>>> GetAsync();

        Task<ActionResponse<IEnumerable<News>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<ActionResponse<IEnumerable<News>>> GetFullAsync(int id);
    }
}
