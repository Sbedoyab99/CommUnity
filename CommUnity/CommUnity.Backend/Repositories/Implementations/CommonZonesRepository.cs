using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class CommonZonesRepository : GenericRepository<CommonZone>, ICommonZonesRepository
    {
        private readonly DataContext _context;
        public CommonZonesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<CommonZone>> GetAsync(int id)
        {
            var commonZone = await _context.CommonZones
                .Include(x => x.ResidentialUnit!)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (commonZone == null)
            {
                return new ActionResponse<CommonZone>
                {
                    WasSuccess = false,
                    Message = "La zona común no existe"
                };
            }

            return new ActionResponse<CommonZone>
            {
                WasSuccess = true,
                Result = commonZone
            };
        }

        public override async Task<ActionResponse<IEnumerable<CommonZone>>> GetAsync()
        {
            var commonZone = await _context.CommonZones
                .OrderBy(x => x.Name)
                .ToListAsync();
            return new ActionResponse<IEnumerable<CommonZone>>
            {
                WasSuccess = true,
                Result = commonZone
            };
        }

        public override async Task<ActionResponse<IEnumerable<CommonZone>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.CommonZones.Include(x => x.ResidentialUnit!).AsQueryable();

            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.ResidentialUnit!.Id == pagination.Id);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<CommonZone>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<CommonZone>>> GetFullAsync(int id)
        {
            var commonZone = await _context.CommonZones
                .OrderBy(x => x.Name)
                .Where(x => x.ResidentialUnit!.Id == id)
                .ToListAsync();
            return new ActionResponse<IEnumerable<CommonZone>>
            {
                WasSuccess = true,
                Result = commonZone
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.CommonZones.Where(x => x.ResidentialUnit!.Id == pagination.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
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
