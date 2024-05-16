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
        private readonly IFileStorage _fileStorage;

        public NewsRepository(DataContext context, IFileStorage fileStorage) : base(context)
        {
            _context = context;
            _fileStorage = fileStorage;
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
                .OrderByDescending(x => x.Date)
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
                    .OrderByDescending(x => x.Date)
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.News.AsQueryable();

            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.ResidentialUnit!.Id == pagination.Id);
            }

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

        public async Task<ActionResponse<int>> GetRecordsNumber(PaginationDTO pagination)
        {
            var queryable = _context.News.AsQueryable();
            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.ResidentialUnit!.Id == pagination.Id);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Title.ToLower().Contains(pagination.Filter.ToLower()));
            }

            int recordsNumber = await queryable.CountAsync();

            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = recordsNumber
            };
        }

        public async Task<ActionResponse<News>> AddFullAsync(NewsDTO newsDTO)
        {
            try
            {
                var newNews = new News
                {
                    Title = newsDTO.Title,
                    Date = newsDTO.Date,
                    Content = newsDTO.Content,
                    ResidentialUnitId = newsDTO.ResidentialUnitId
                };

                if (!string.IsNullOrWhiteSpace(newsDTO.Picture))
                {
                    var photo = Convert.FromBase64String(newsDTO.Picture);
                    newNews.Picture = await _fileStorage.SaveFileAsync(photo, ".jpg", "products");
                }
                
                _context.Add(newNews);
                await _context.SaveChangesAsync();
                return new ActionResponse<News>
                {
                    WasSuccess = true,
                    Result = newNews
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<News>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };

            }
        }

        public async Task<ActionResponse<News>> UpdateFullAsync(NewsDTO newsDTO)
        {
            try
            {
                var news = await _context.News.FirstOrDefaultAsync(x => x.Id == newsDTO.Id);

                if (news == null)
                {
                    return new ActionResponse<News>
                    {
                        WasSuccess = false,
                        Message = "Noticia no existe"
                    };
                }

                news.Title = newsDTO.Title;
                news.Date = newsDTO.Date;
                news.Content = newsDTO.Content;
                news.ResidentialUnitId = newsDTO.ResidentialUnitId;

                if(!string.IsNullOrWhiteSpace(news.Picture) && !string.IsNullOrWhiteSpace(newsDTO.Picture))
                {
                    await _fileStorage.RemoveFileAsync(news.Picture, "products");
                    var photo = Convert.FromBase64String(newsDTO.Picture);
                    news.Picture = await _fileStorage.SaveFileAsync(photo, ".jpg", "products");
                } 
                else if (string.IsNullOrWhiteSpace(news.Picture) && !string.IsNullOrWhiteSpace(newsDTO.Picture))
                {
                    var photo = Convert.FromBase64String(newsDTO.Picture);
                    news.Picture = await _fileStorage.SaveFileAsync(photo, ".jpg", "products");
                }

                _context.Update(news);
                await _context.SaveChangesAsync();
                return new ActionResponse<News>
                {
                    WasSuccess = true,
                    Result = news
                };
            }
            catch (DbUpdateException)
            {
                return new ActionResponse<News>
                {
                    WasSuccess = false,
                    Message = "Ya existe una noticia con el mismo nombre."
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<News>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };
            }
        }
    }
}