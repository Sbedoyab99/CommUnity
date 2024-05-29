using CommUnity.BackEnd.Repositories.Implementations;
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

        public async Task<ActionResponse<Pet>> AddFullAsync(PetDTO petDTO) => await _PetsRepository.AddFullAsync(petDTO);

        public async Task<ActionResponse<Pet>> UpdateFullAsync(PetDTO petDTO) => await _PetsRepository.UpdateFullAsync(petDTO);

        public override async Task<ActionResponse<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination) => await _PetsRepository.GetAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _PetsRepository.GetTotalPagesAsync(pagination);

        public async Task<ActionResponse<int>> GetRecordsNumber(PaginationDTO pagination) => await _PetsRepository.GetRecordsNumber(pagination);
    }
}