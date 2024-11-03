using CommUnity.BackEnd.Repositories.Implementations;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class PqrsUnitOfWork : GenericUnitOfWork<Pqrs>, IPqrsUnitOfWork
    {
        private readonly IPqrsRepository _pqrsRepository;

        public PqrsUnitOfWork(IGenericRepository<Pqrs> repository, IPqrsRepository pqrsRepository) : base(repository)
        {
            _pqrsRepository = pqrsRepository;
        }

        public async Task<ActionResponse<Pqrs>> CreatePqrs(string email, PqrsDTO pqrsDTO) => await _pqrsRepository.CreatePqrs(email, pqrsDTO);

        public async Task<ActionResponse<IEnumerable<Pqrs>>> GetPqrsByTypeByStatus(string email, PqrsType type, PqrsState status) => await _pqrsRepository.GetPqrsByTypeByStatus(email, type,status);

        public async Task<ActionResponse<int>> GetPqrsRecordsNumber(string email, int id, PqrsType type, PqrsState status) => await _pqrsRepository.GetPqrsRecordsNumber(email, id, type,status);
    }
}
