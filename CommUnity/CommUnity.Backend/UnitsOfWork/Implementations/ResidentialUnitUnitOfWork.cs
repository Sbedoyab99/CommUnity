using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class ResidentialUnitUnitOfWork : GenericUnitOfWork<ResidentialUnit>, IResidentialUnitUnitOfWork
    {
        private readonly IResidentialUnitRepository _residentialUnitRepository;

        public ResidentialUnitUnitOfWork(IGenericRepository<ResidentialUnit> repository, IResidentialUnitRepository residentialUnitRepository) : base(repository)
        {
            _residentialUnitRepository = residentialUnitRepository;
        }

        public override async Task<ActionResponse<IEnumerable<ResidentialUnit>>> GetAsync() => await _residentialUnitRepository.GetAsync();

        public override async Task<ActionResponse<ResidentialUnit>> GetAsync(int id) => await _residentialUnitRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<ResidentialUnit>>> GetAsync(PaginationDTO pagination) => await _residentialUnitRepository.GetAsync(pagination);

        public Task<ActionResponse<IEnumerable<ResidentialUnit>>> GetFullAsync(int id) => _residentialUnitRepository.GetFullAsync(id);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _residentialUnitRepository.GetTotalPagesAsync(pagination);
    }
}