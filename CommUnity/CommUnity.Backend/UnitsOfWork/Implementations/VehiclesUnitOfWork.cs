using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class VehiclesUnitOfWork : GenericUnitOfWork<Vehicle>, IVehiclesUnitOfWork
    {
        private readonly IVehiclesRepository _VehiclesRepository;

        public VehiclesUnitOfWork(IGenericRepository<Vehicle> repository, IVehiclesRepository VehiclesRepository) : base(repository)
        {
            _VehiclesRepository = VehiclesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync() => await _VehiclesRepository.GetAsync();

        public override async Task<ActionResponse<Vehicle>> GetAsync(int id) => await _VehiclesRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync(PaginationDTO pagination) => await _VehiclesRepository.GetAsync(pagination);

        public Task<ActionResponse<IEnumerable<Vehicle>>> GetFullAsync(int id) => _VehiclesRepository.GetFullAsync(id);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _VehiclesRepository.GetTotalPagesAsync(pagination);

    }
}
