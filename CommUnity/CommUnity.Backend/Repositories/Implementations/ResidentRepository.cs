using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class ResidentRepository : GenericRepository<User>, IResidentRepository
    {
        private readonly DataContext _context;

        public ResidentRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public async Task<ActionResponse<IEnumerable<User>>> GetResidentAsync(int apartmentId)
        {
            var queryable = _context.Users.Where(x => x.ApartmentId == apartmentId).AsQueryable();

            return new ActionResponse<IEnumerable<User>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Document)
                    .ToListAsync()
            };
        }
    }
}