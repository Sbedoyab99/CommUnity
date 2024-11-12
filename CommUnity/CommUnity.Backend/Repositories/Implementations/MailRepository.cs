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
    public class MailRepository : GenericRepository<MailArrival>, IMailRepository
    {
        private readonly DataContext _context;
        private readonly IUsersRepository _usersRepository;

        public MailRepository(DataContext context, IUsersRepository usersRepository) : base(context)
        {
            _context = context;
            _usersRepository = usersRepository;
        }

        public async Task<ActionResponse<MailArrival>> ConfirmMail(string email, MailArrivalDTO mailArrivalDTO)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var mailArrival = await _context.MailArrivals
                .FirstOrDefaultAsync(x => x.Id == mailArrivalDTO.Id);
            if (mailArrival == null)
            {
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = false,
                    Message = "Correspondencia no existe."
                };
            }
            mailArrival.Sender = mailArrivalDTO.Sender;
            mailArrival.Type = mailArrivalDTO.Type;
            mailArrival.DateTime = DateTime.Now.Date;
            mailArrival.Status = mailArrivalDTO.Status;

            _context.MailArrivals.Update(mailArrival);
            await _context.SaveChangesAsync();
            return new ActionResponse<MailArrival>
            {
                WasSuccess = true,
                Result = mailArrival
            };
        }

        public async Task<ActionResponse<MailArrival>> UpdateStatusMail(string email, MailArrivalDTO mailArrivalDTO)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var mailArrival = await _context.MailArrivals
                .FirstOrDefaultAsync(x => x.Id == mailArrivalDTO.Id);
            if (mailArrival == null)
            {
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = false,
                    Message = "Correspondencia no existe."
                };
            }
            mailArrival.Status = mailArrivalDTO.Status;

            _context.MailArrivals.Update(mailArrival);
            await _context.SaveChangesAsync();
            return new ActionResponse<MailArrival>
            {
                WasSuccess = true,
                Result = mailArrival
            };
        }

        public async Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByApartment(string email, int apartmentId)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<MailArrival>>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var queryable = _context.MailArrivals.Where(x => x.ApartmentId == apartmentId).Select(x => new MailArrival
            {
                Id = x.Id,
                Sender = x.Sender,
                Type = x.Type,
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

            return new ActionResponse<IEnumerable<MailArrival>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.DateTime)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByStatus(string email, MailStatus status)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<MailArrival>>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var queryable = _context.MailArrivals.Where(x => x.ResidentialUnitId == user.ResidentialUnitId && x.Status == status).Select(x => new MailArrival
            {
                Id = x.Id,
                Sender = x.Sender,
                Type = x.Type,
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

            return new ActionResponse<IEnumerable<MailArrival>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.DateTime)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<MailArrival>>> GetMailByAparmentStatus(string email, PaginationMailDTO paginationMailDTO)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<MailArrival>>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var queryable = _context.MailArrivals.Where(x => x.ApartmentId == paginationMailDTO.Id && x.Status == paginationMailDTO.Status).Select(x => new MailArrival
            {
                Id = x.Id,
                Sender = x.Sender,
                Type = x.Type,
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

            return new ActionResponse<IEnumerable<MailArrival>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.DateTime)
                    .Paginate(paginationMailDTO)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<int>> GetMailRecordsNumber(string email, int id, MailStatus status)
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

            var queryable = _context.MailArrivals.Where(x => x.ResidentialUnitId == user.ResidentialUnitId && x.Status == status).AsQueryable();

            int recordsNumber = await queryable.CountAsync();

            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = recordsNumber
            };
        }

        public async Task<ActionResponse<int>> GetMailRecordsNumberApartment(string email, PaginationMailDTO paginationMailDTO)
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

            var queryable = _context.MailArrivals.AsQueryable();

            if (paginationMailDTO.Id != 0)
            {
                queryable = queryable.Where(x => x.ApartmentId == paginationMailDTO.Id);
            }

            queryable = queryable.Where(x => x.Status == paginationMailDTO.Status);

            //var queryable = _context.MailArrivals.Where(x => x.ResidentialUnitId == user.ResidentialUnitId && x.Status == status).AsQueryable();

            int recordsNumber = await queryable.CountAsync();

            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = recordsNumber
            };
        }

        public async Task<ActionResponse<MailArrival>> RegisterMail(string email, MailArrivalDTO mailArrivalDTO)
        {
            var user = await _usersRepository.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = false,
                    Message = "Usuario no existe"
                };
            }

            var isWorker = await _usersRepository.IsUserInRoleAsync(user, UserType.Worker.ToString());

            if (!isWorker)
            {
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = false,
                    Message = "Solo permitido para trabajadores."
                };
            }

            var mailArrival = new MailArrival
            {
                Sender = mailArrivalDTO.Sender,
                Type = mailArrivalDTO.Type,
                Status = MailStatus.Stored,
                DateTime = DateTime.Now.Date,
                ResidentialUnitId = user.ResidentialUnitId,
                ApartmentId = mailArrivalDTO.ApartmentId
            };

            try
            {
                _context.MailArrivals.Add(mailArrival);
                await _context.SaveChangesAsync();
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = true,
                    Result = mailArrival
                };
            }
            catch (Exception ex)
            {
                return new ActionResponse<MailArrival>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
