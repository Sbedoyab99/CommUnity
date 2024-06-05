using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
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

        public async Task<ActionResponse<VisitorEntry>> ScheduleVisitor(string email, VisitorEntryDTO visitorEntryDTO) => await _visitorEntryRepository.ScheduleVisitor(email, visitorEntryDTO);
    }
}
