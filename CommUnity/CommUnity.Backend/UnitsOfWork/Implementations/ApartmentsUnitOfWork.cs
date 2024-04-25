using CommUnity.BackEnd.Repositories.Implementations;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class ApartmentsUnitOfWork : GenericUnitOfWork<Apartment>, IApartmentsUnitOfWork
    {
        private readonly IApartmentsRepository _apartmentsRepository;

        public ApartmentsUnitOfWork(IGenericRepository<Apartment> repository, IApartmentsRepository apartmentsRepository) : base(repository)
        {
            _apartmentsRepository = apartmentsRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Apartment>>> GetAsync() => await _apartmentsRepository.GetAsync();

        public override async Task<ActionResponse<Apartment>> GetAsync(int id) => await _apartmentsRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<Apartment>>> GetAsync(PaginationDTO pagination) => await _apartmentsRepository.GetAsync(pagination);

        public Task<ActionResponse<IEnumerable<Apartment>>> GetFullAsync(int id) => _apartmentsRepository.GetFullAsync(id);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _apartmentsRepository.GetTotalPagesAsync(pagination);
    }
}