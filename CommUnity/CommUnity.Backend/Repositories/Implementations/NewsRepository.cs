using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Repositories.Implementations
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        private readonly DataContext _context;
        public NewsRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<News>> GetAsync(int id)
        {
            var news = await _context.News
                .Include(x => x.ResidentialUnit!)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (news == null)
            {
                return new ActionResponse<News>
                {
                    WasSuccess = false,
                    Message = "La noticia no existe"
                };
            }

            return new ActionResponse<News>
            {
                WasSuccess = true,
                Result = news
            };
        }

        public override async Task<ActionResponse<IEnumerable<News>>> GetAsync()
        {
            var news = await _context.News
                .OrderBy(x => x.Title)
                .ToListAsync();
            return new ActionResponse<IEnumerable<News>>
            {
                WasSuccess = true,
                Result = news
            };
        }

        public override async Task<ActionResponse<IEnumerable<News>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.News.Include(x => x.ResidentialUnit!).AsQueryable();

            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.ResidentialUnit!.Id == pagination.Id);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Title.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<News>>
            {
                WasSuccess = true,
                Result = await queryable
                    .OrderBy(x => x.Title)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public async Task<ActionResponse<IEnumerable<News>>> GetFullAsync(int id)
        {
            var news = await _context.News
                .OrderBy(x => x.Title)
                .Where(x => x.ResidentialUnit!.Id == id)
                .ToListAsync();
            return new ActionResponse<IEnumerable<News>>
            {
                WasSuccess = true,
                Result = news
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.News.Where(x => x.ResidentialUnit!.Id == pagination.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Title.ToLower().Contains(pagination.Filter.ToLower()));
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
