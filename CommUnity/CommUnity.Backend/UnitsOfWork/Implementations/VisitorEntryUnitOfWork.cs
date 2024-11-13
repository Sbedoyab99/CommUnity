using CommUnity.BackEnd.Repositories.Implementations;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.UnitsOfWork.Implementations
{
    public class VisitorEntryUnitOfWork : GenericUnitOfWork<VisitorEntry>, IVisitorEntryUnitOfWork
    {
        private readonly IVisitorEntryRepository _visitorEntryRepository;

        public VisitorEntryUnitOfWork(IGenericRepository<VisitorEntry> repository, IVisitorEntryRepository visitorEntryRepository) : base(repository)
        {
            _visitorEntryRepository = visitorEntryRepository;
        }

        public async Task<ActionResponse<IEnumerable<VisitorEntry>>> GetVisitorEntryByStatus(string email, VisitorStatus status) => await _visitorEntryRepository.GetVisitorEntryByStatus(email, status);

        public async Task<ActionResponse<VisitorEntry>> ScheduleVisitor(string email, VisitorEntryDTO visitorEntryDTO) => await _visitorEntryRepository.ScheduleVisitor(email, visitorEntryDTO);

        public async Task<ActionResponse<VisitorEntry>> ConfirmVisitorEntry(string email, VisitorEntryDTO visitorEntryDTO) => await _visitorEntryRepository.ConfirmVisitorEntry(email, visitorEntryDTO);

        public async Task<ActionResponse<VisitorEntry>> CancelVisitorEntry(string email, VisitorEntryDTO visitorEntryDTO) => await _visitorEntryRepository.CancelVisitorEntry(email, visitorEntryDTO);

        public async Task<ActionResponse<VisitorEntry>> AddVisitor(string email, VisitorEntryDTO visitorEntryDTO) => await _visitorEntryRepository.AddVisitor(email, visitorEntryDTO);

        public async Task<ActionResponse<IEnumerable<VisitorEntry>>> GetVisitorEntryByApartment(string email, int apartmentId) => await _visitorEntryRepository.GetVisitorEntryByApartment(email, apartmentId);

        public async Task<ActionResponse<int>> GetVisitorEntryRecordsNumber(string email, int id, VisitorStatus status) => await _visitorEntryRepository.GetVisitorEntryRecordsNumber(email, id, status);

        public async Task<ActionResponse<int>> GetVisitorRecordsNumberApartment(string email, PaginationVisitorDTO paginationVisitorDTO) => await _visitorEntryRepository.GetVisitorRecordsNumberApartment(email, paginationVisitorDTO);

        public async Task<ActionResponse<int>> GetVisitorRecordsNumberResidentialUnit(string email, PaginationVisitorDTO paginationVisitorDTO)=> await _visitorEntryRepository.GetVisitorRecordsNumberResidentialUnit(email, paginationVisitorDTO);

        public async Task<ActionResponse<IEnumerable<VisitorEntry>>> GetVisitorEntryByAparmentStatus(string email, PaginationVisitorDTO paginationVisitorDTO) => await _visitorEntryRepository.GetVisitorEntryByAparmentStatus(email, paginationVisitorDTO);

        public async Task<ActionResponse<IEnumerable<VisitorEntry>>> GetVisitorByResidentialUnitStatus(string email, PaginationVisitorDTO paginationVisitorDTO) => await _visitorEntryRepository.GetVisitorByResidentialUnitStatus(email, paginationVisitorDTO);
    }
}
