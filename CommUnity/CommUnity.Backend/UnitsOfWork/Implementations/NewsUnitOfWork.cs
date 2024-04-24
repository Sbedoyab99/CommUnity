using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class NewsUnitOfWork : GenericUnitOfWork<News>, INewsUnitOfWork
    {
        private readonly INewsRepository _NewsRepository;

        public NewsUnitOfWork(IGenericRepository<News> repository, INewsRepository NewsRepository) : base(repository)
        {
            _NewsRepository = NewsRepository;
        }

        public override async Task<ActionResponse<IEnumerable<News>>> GetAsync() => await _NewsRepository.GetAsync();

        public override async Task<ActionResponse<News>> GetAsync(int id) => await _NewsRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<News>>> GetAsync(PaginationDTO pagination) => await _NewsRepository.GetAsync(pagination);

        public Task<ActionResponse<IEnumerable<News>>> GetFullAsync(int id) => _NewsRepository.GetFullAsync(id);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _NewsRepository.GetTotalPagesAsync(pagination);

    }
}
