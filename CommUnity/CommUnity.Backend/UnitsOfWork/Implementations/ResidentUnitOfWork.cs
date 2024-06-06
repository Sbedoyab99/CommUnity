using CommUnity.BackEnd.Repositories.Implementations;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class ResidentUnitOfWork : GenericUnitOfWork<User>, IResidentUnitOfWork
    {
        private readonly IResidentRepository _ResidentRepository;

        public ResidentUnitOfWork(IGenericRepository<User> repository, IResidentRepository ResidentRepository) : base(repository)
        {
            _ResidentRepository = ResidentRepository;
        }

        public async Task<ActionResponse<IEnumerable<User>>> GetResidentAsync(int apartmentId) => await _ResidentRepository.GetResidentAsync(apartmentId);

    }
}
