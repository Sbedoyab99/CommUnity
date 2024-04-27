using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class PetsRepository : GenericRepository<Pet>, IPetsRepository
    {
        private readonly DataContext _context;

        public PetsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<Pet>> GetAsync(int id)
        {
            var pets = await _context.Pets
                .Include(x => x.Apartment!)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pets == null)
            {
                return new ActionResponse<Pet>
                {
                    WasSuccess = false,
                    Message = "La mascota no existe"
                };
            }

            return new ActionResponse<Pet>
            {
                WasSuccess = true,
                Result = pets
            };
        }

        public override async Task<ActionResponse<IEnumerable<Pet>>> GetAsync()
        {
            var pets = await _context.Pets
                .OrderBy(x => x.Name)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Pet>>
            {
                WasSuccess = true,
                Result = pets
            };
        }

        public override async Task<ActionResponse<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Pets.Include(x => x.Apartment!).AsQueryable();

            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.Apartment!.Id == pagination.Id);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Pet>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Name)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<Pet>>> GetFullAsync(int id)
        {
            var pets = await _context.Pets
                .OrderBy(x => x.Name)
                .Where(x => x.Apartment!.Id == id)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Pet>>
            {
                WasSuccess = true,
                Result = pets
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Pets.Where(x => x.Apartment!.Id == pagination.Id).AsQueryable();

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