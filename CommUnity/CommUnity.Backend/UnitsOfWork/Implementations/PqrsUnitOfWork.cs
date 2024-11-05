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
        public override async Task<ActionResponse<Pqrs>> GetAsync(int id) => await _pqrsRepository.GetAsync(id);

        public async Task<ActionResponse<Pqrs>> CreatePqrs(string email, PqrsDTO pqrsDTO) => await _pqrsRepository.CreatePqrs(email, pqrsDTO);

        public async Task<ActionResponse<IEnumerable<Pqrs>>> GetPqrsByTypeByStatus(string email, PqrsType type, PqrsState status) => await _pqrsRepository.GetPqrsByTypeByStatus(email, type, status);

        public async Task<ActionResponse<IEnumerable<Pqrs>>> GetPqrsByResidentialUnitByTypeByStatus(string email, int residentialUnitId, PqrsType type, PqrsState status) => await _pqrsRepository.GetPqrsByResidentialUnitByTypeByStatus(email, residentialUnitId, type, status);

        public async Task<ActionResponse<int>> GetPqrsRecordsNumber(string email, int id, PqrsType type, PqrsState status) => await _pqrsRepository.GetPqrsRecordsNumber(email, id, type, status);

        public async Task<ActionResponse<int>> GetPqrsAdminRecordsNumber(string email, int id, PqrsType type, PqrsState status) => await _pqrsRepository.GetPqrsAdminRecordsNumber(email, id, type, status);

        public async Task<ActionResponse<Pqrs>> UpdatePqrs(PqrsDTO pqrsDTO) => await _pqrsRepository.UpdatePqrs(pqrsDTO);

        public async Task<ActionResponse<Pqrs>> UpdateStatusPqrs(PqrsDTO pqrsDTO) => await _pqrsRepository.UpdateStatusPqrs(pqrsDTO);
    }
}
