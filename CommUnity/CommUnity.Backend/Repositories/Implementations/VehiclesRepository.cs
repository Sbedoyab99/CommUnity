using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class VehiclesRepository : GenericRepository<Vehicle>, IVehiclesRepository
    {
        private readonly DataContext _context;
        public VehiclesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<Vehicle>> GetAsync(int id)
        {
            var Vehicles = await _context.Vehicles
                .Include(x => x.Apartment!)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Vehicles == null)
            {
                return new ActionResponse<Vehicle>
                {
                    WasSuccess = false,
                    Message = "El vehiculo no existe"
                };
            }

            return new ActionResponse<Vehicle>
            {
                WasSuccess = true,
                Result = Vehicles
            };
        }

        public override async Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync()
        {
            var Vehicles = await _context.Vehicles
                .OrderBy(x => x.Plate)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Vehicle>>
            {
                WasSuccess = true,
                Result = Vehicles
            };
        }

        public override async Task<ActionResponse<IEnumerable<Vehicle>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Vehicles.Include(x => x.Apartment!).AsQueryable();

            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.Apartment!.Id == pagination.Id);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Plate.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Vehicle>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Plate)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<Vehicle>>> GetFullAsync(int id)
        {
            var Vehicles = await _context.Vehicles
                .OrderBy(x => x.Plate)
                .Where(x => x.Apartment!.Id == id)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Vehicle>>
            {
                WasSuccess = true,
                Result = Vehicles
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Vehicles.Where(x => x.Apartment!.Id == pagination.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Plate.ToLower().Contains(pagination.Filter.ToLower()));
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
