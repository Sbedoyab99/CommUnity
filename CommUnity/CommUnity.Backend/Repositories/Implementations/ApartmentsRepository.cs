using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class ApartmentsRepository : GenericRepository<Apartment>, IApartmentsRepository
    {
        private readonly DataContext _context;

        public ApartmentsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<Apartment>> GetAsync(int id)
        {
            var apartment = await _context.Apartments
                .Include(x => x.ResidentialUnit!)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (apartment == null)
            {
                return new ActionResponse<Apartment>
                {
                    WasSuccess = false,
                    Message = "El Apartamento no existe"
                };
            }

            return new ActionResponse<Apartment>
            {
                WasSuccess = true,
                Result = apartment
            };
        }

        public override async Task<ActionResponse<IEnumerable<Apartment>>> GetAsync()
        {
            var apartments = await _context.Apartments
                .OrderBy(x => x.Number)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Apartment>>
            {
                WasSuccess = true,
                Result = apartments
            };
        }

        public override async Task<ActionResponse<IEnumerable<Apartment>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Apartments.Include(x => x.ResidentialUnit!).Include(x => x.Vehicles!).Include(x => x.Pets!).AsQueryable();

            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.ResidentialUnit!.Id == pagination.Id);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Number.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Apartment>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Number)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<Apartment>>> GetFullAsync(int id)
        {
            var apartments = await _context.Apartments
                .OrderBy(x => x.Number)
                .Where(x => x.ResidentialUnit!.Id == id)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Apartment>>
            {
                WasSuccess = true,
                Result = apartments
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Apartments.Where(x => x.ResidentialUnit!.Id == pagination.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Number.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }
    }
}
