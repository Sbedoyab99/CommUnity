using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class CommonZonesUnitOfWork : GenericUnitOfWork<CommonZone>, ICommonZonesUnitOfWork
    {
        private readonly ICommonZonesRepository _CommonZonesRepository;

        public CommonZonesUnitOfWork(IGenericRepository<CommonZone> repository, ICommonZonesRepository CommonZonesRepository) : base(repository)
        {
            _CommonZonesRepository = CommonZonesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<CommonZone>>> GetAsync() => await _CommonZonesRepository.GetAsync();

        public override async Task<ActionResponse<CommonZone>> GetAsync(int id) => await _CommonZonesRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<CommonZone>>> GetAsync(PaginationDTO pagination) => await _CommonZonesRepository.GetAsync(pagination);

        public Task<ActionResponse<IEnumerable<CommonZone>>> GetFullAsync(int id) => _CommonZonesRepository.GetFullAsync(id);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _CommonZonesRepository.GetTotalPagesAsync(pagination);

    }
}
