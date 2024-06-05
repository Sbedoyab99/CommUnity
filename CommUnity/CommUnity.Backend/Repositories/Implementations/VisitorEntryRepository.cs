using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class VisitorEntryRepository : GenericRepository<VisitorEntry>, IVisitorEntryRepository
    {
        private readonly DataContext _context;
        private readonly IUsersRepository _usersRepository;

        public VisitorEntryRepository(DataContext context, IUsersRepository usersRepository) : base(context)
        {
            _context = context;
            _usersRepository = usersRepository;
        }

        public async Task<ActionResponse<VisitorEntry>> ScheduleVisitor(string email, VisitorEntryDTO visitorEntryDTO)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<VisitorEntry>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var isResident = await _usersRepository.IsUserInRoleAsync(user, UserType.Resident.ToString());

            if (!isResident)
            {
                return new ActionResponse<VisitorEntry>
                {
                    WasSuccess = false,
                    Message = "Solo permitido para residentes."
                };
            }

            var visitorEntry = new VisitorEntry
            {
                Name = visitorEntryDTO.Name,
                Plate = visitorEntryDTO.Plate,
                Status = VisitorStatus.Scheduled,
                DateTime = visitorEntryDTO.Date,
                ResidentialUnitId = user.ResidentialUnitId,
                ApartmentId = user.ApartmentId
            };

            try
            {
                _context.VisitorEntries.Add(visitorEntry);
                await _context.SaveChangesAsync();
                return new ActionResponse<VisitorEntry>
                {
                    WasSuccess = true,
                    Result = visitorEntry
                };
            }
            catch (Exception ex)
            {
                return new ActionResponse<VisitorEntry>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
