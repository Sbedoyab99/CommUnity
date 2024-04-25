using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class PetsUnitOfWork : GenericUnitOfWork<Pet>, IPetsUnitOfWork
    {
        private readonly IPetsRepository _PetsRepository;

        public PetsUnitOfWork(IGenericRepository<Pet> repository, IPetsRepository PetsRepository) : base(repository)
        {
            _PetsRepository = PetsRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Pet>>> GetAsync() => await _PetsRepository.GetAsync();

        public override async Task<ActionResponse<Pet>> GetAsync(int id) => await _PetsRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination) => await _PetsRepository.GetAsync(pagination);

        public Task<ActionResponse<IEnumerable<Pet>>> GetFullAsync(int id) => _PetsRepository.GetFullAsync(id);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _PetsRepository.GetTotalPagesAsync(pagination);

    }
}
