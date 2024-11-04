using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class PqrsRepository : GenericRepository<Pqrs>, IPqrsRepository
    {
        private readonly DataContext _context;
        private readonly IUsersRepository _usersRepository;

        public PqrsRepository(DataContext context, IUsersRepository usersRepository) : base(context)
        {
            _context = context;
            _usersRepository = usersRepository;
        }

        public override async Task<ActionResponse<Pqrs>> GetAsync(int id)
        {
            var pqrss = await _context.Pqrss
                .Include(x => x.Apartment!)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pqrss == null)
            {
                return new ActionResponse<Pqrs>
                {
                    WasSuccess = false,
                    Message = "La pqrs no existe"
                };
            }

            return new ActionResponse<Pqrs>
            {
                WasSuccess = true,
                Result = pqrss
            };
        }

        public async Task<ActionResponse<Pqrs>> CreatePqrs(string email, PqrsDTO pqrsDTO)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<Pqrs>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var isResident = await _usersRepository.IsUserInRoleAsync(user, UserType.Resident.ToString());
            if (!isResident)
            {
                return new ActionResponse<Pqrs>
                {
                    WasSuccess = false,
                    Message = "Solo permitido para residentes."
                };
            }

            var pqrs = new Pqrs
            {
                DateTime = DateTime.Now,
                PqrsType = pqrsDTO.Type,
                Content = pqrsDTO.Content,
                PqrsState = pqrsDTO.Status,
                ApartmentId = pqrsDTO.ApartmentId,
                ResidentialUnitId = pqrsDTO.ResidentialUnitId
            };

            try
            {
                _context.Pqrss.Add(pqrs);
                await _context.SaveChangesAsync();
                return new ActionResponse<Pqrs>
                {
                    WasSuccess = true,
                    Result = pqrs
                };

            }
            catch (Exception ex)
            {

                return new ActionResponse<Pqrs>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<ActionResponse<IEnumerable<Pqrs>>> GetPqrsByTypeByStatus(string email, PqrsType type, PqrsState status)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<Pqrs>>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var queryable = _context.Pqrss.Where(x => x.ApartmentId == user.ApartmentId && x.PqrsType == type && x.PqrsState == status).Select(x => new Pqrs
            {
                Id = x.Id,
                DateTime = x.DateTime,
                PqrsType = x.PqrsType,
                Content = x.Content,
                PqrsState = x.PqrsState,
                Apartment = new Apartment
                {
                    Id = x.Apartment!.Id,
                    Number = x.Apartment.Number
                }
            });

            return new ActionResponse<IEnumerable<Pqrs>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.DateTime)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<int>> GetPqrsRecordsNumber(string email, int id, PqrsType type, PqrsState status)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<int>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var queryable = _context.Pqrss.Where(x => x.ApartmentId == user.ApartmentId && x.PqrsType == type && x.PqrsState == status).AsQueryable();

            int recordsNumber = await queryable.CountAsync();

            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = recordsNumber
            };
        }

        public async Task<ActionResponse<Pqrs>> UpdatePqrs(PqrsDTO pqrsDTO)
        {

            var pqrs = await _context.Pqrss
                .FirstOrDefaultAsync(x => x.Id == pqrsDTO.Id);
            if (pqrs == null)
            {
                return new ActionResponse<Pqrs>
                {
                    WasSuccess = false,
                    Message = "Pqrs no existe"
                };
            }
            pqrs.PqrsType = pqrsDTO.Type;
            pqrs.Content = pqrsDTO.Content;

            _context.Pqrss.Update(pqrs);
            await _context.SaveChangesAsync();
            return new ActionResponse<Pqrs>
            {
                WasSuccess = true,
                Result = pqrs
            };
        }

        public Task<ActionResponse<User>> GetAdmiResidentialUnit(int residentialUnitId)
        {
            throw new NotImplementedException();
        }
    }
}
