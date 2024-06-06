using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ActionResponse<IEnumerable<VisitorEntry>>> GetVisitorEntryByStatus(string email, VisitorStatus status)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<VisitorEntry>>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var queryable = _context.VisitorEntries.Where(x => x.ResidentialUnitId == user.ResidentialUnitId && x.Status == status).Select(x => new VisitorEntry
            {
                Id = x.Id,
                Name = x.Name,
                Plate = x.Plate,
                DateTime = x.DateTime,
                Status = x.Status,
                ResidentialUnit = new ResidentialUnit
                {
                    Id = x.ResidentialUnit!.Id,
                    Name = x.ResidentialUnit.Name
                },
                Apartment = new Apartment
                {
                    Id = x.Apartment!.Id,
                    Number = x.Apartment.Number
                }
            });

            return new ActionResponse<IEnumerable<VisitorEntry>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.DateTime)
                    .ToListAsync()
            };
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

        public async Task<ActionResponse<VisitorEntry>> ConfirmVisitorEntry(string email, VisitorEntryDTO visitorEntryDTO)
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
         
            var visitorEntry = await _context.VisitorEntries
                .FirstOrDefaultAsync(x => x.Id == visitorEntryDTO.Id);
            if(visitorEntry == null)
            {
                return new ActionResponse<VisitorEntry>
                {
                    WasSuccess = false,
                    Message = "Visitante no existe."
                };
            }
            visitorEntry.Name = visitorEntryDTO.Name;
            visitorEntry.Plate = visitorEntryDTO.Plate;
            visitorEntry.DateTime = visitorEntryDTO.Date;
            visitorEntry.Status = visitorEntryDTO.Status;

            _context.VisitorEntries.Update(visitorEntry);
            await _context.SaveChangesAsync();
            return new ActionResponse<VisitorEntry>
            {
                WasSuccess = true,
                Result = visitorEntry
            };
        }

        public async Task<ActionResponse<VisitorEntry>> CancelVisitorEntry(string email, VisitorEntryDTO visitorEntryDTO)
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

            var visitorEntry = await _context.VisitorEntries
                .FirstOrDefaultAsync(x => x.Id == visitorEntryDTO.Id);
            if (visitorEntry == null)
            {
                return new ActionResponse<VisitorEntry>
                {
                    WasSuccess = false,
                    Message = "Visitante no existe."
                };
            }
            visitorEntry.Name = visitorEntryDTO.Name;
            visitorEntry.Plate = visitorEntryDTO.Plate;
            visitorEntry.DateTime = visitorEntryDTO.Date;
            visitorEntry.Status = visitorEntryDTO.Status;

            _context.VisitorEntries.Update(visitorEntry);
            await _context.SaveChangesAsync();
            return new ActionResponse<VisitorEntry>
            {
                WasSuccess = true,
                Result = visitorEntry
            };
        }

        public async Task<ActionResponse<VisitorEntry>> AddVisitor(string email, VisitorEntryDTO visitorEntryDTO)
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

            var isWorker = await _usersRepository.IsUserInRoleAsync(user, UserType.Worker.ToString());

            if (!isWorker)
            {
                return new ActionResponse<VisitorEntry>
                {
                    WasSuccess = false,
                    Message = "Solo permitido para trabajadores."
                };
            }

            var visitorEntry = new VisitorEntry
            {
                Name = visitorEntryDTO.Name,
                Plate = visitorEntryDTO.Plate,
                Status = VisitorStatus.Approved,
                DateTime = visitorEntryDTO.Date,
                ResidentialUnitId = user.ResidentialUnitId,
                ApartmentId = visitorEntryDTO.ApartmentId
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

        public async Task<ActionResponse<IEnumerable<VisitorEntry>>> GetVisitorEntryByApartment(string email, int apartmentId)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<VisitorEntry>>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var queryable = _context.VisitorEntries.Where(x => x.ApartmentId == apartmentId).Select(x => new VisitorEntry
            {
                Id = x.Id,
                Name = x.Name,
                Plate = x.Plate,
                DateTime = x.DateTime,
                Status = x.Status,
                ResidentialUnit = new ResidentialUnit
                {
                    Id = x.ResidentialUnit!.Id,
                    Name = x.ResidentialUnit.Name
                },
                Apartment = new Apartment
                {
                    Id = x.Apartment!.Id,
                    Number = x.Apartment.Number
                }
            });

            return new ActionResponse<IEnumerable<VisitorEntry>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.DateTime)
                    .ToListAsync()
            };
        }
    }
}
