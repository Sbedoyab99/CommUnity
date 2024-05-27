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
        private readonly IFileStorage _fileStorage;

        public PetsRepository(DataContext context, IFileStorage fileStorage) : base(context)
        {
            _context = context;
            _fileStorage = fileStorage;
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

        public async Task<ActionResponse<int>> GetRecordsNumber(PaginationDTO pagination)
        {
            var queryable = _context.Pets.AsQueryable();
            if (pagination.Id != 0)
            {
                queryable = queryable.Where(x => x.Apartment!.Id == pagination.Id);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            int recordsNumber = await queryable.CountAsync();

            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = recordsNumber
            };
        }

        public async Task<ActionResponse<Pet>> AddFullAsync(PetDTO petDTO)
        {
            try
            {
                var pet = new Pet
                {
                    Name = petDTO.Name,
                    Breed = petDTO.Breed,
                    Picture = petDTO.Picture,
                    ApartmentId = petDTO.ApartmentId
                };

                if (!string.IsNullOrWhiteSpace(petDTO.Picture))
                {
                    var photo = Convert.FromBase64String(petDTO.Picture);
                    pet.Picture = await _fileStorage.SaveFileAsync(photo, ".jpg", "pets");
                }

                _context.Add(pet);
                await _context.SaveChangesAsync();
                return new ActionResponse<Pet>
                {
                    WasSuccess = true,
                    Result = pet
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<Pet>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };

            }
        }

        public async Task<ActionResponse<Pet>> UpdateFullAsync(PetDTO petDTO)
        {
            try
            {
                var pet = await _context.Pets.FirstOrDefaultAsync(x => x.Id == petDTO.Id);

                if (pet == null)
                {
                    return new ActionResponse<Pet>
                    {
                        WasSuccess = false,
                        Message = "Mascota no existe"
                    };
                }

                pet.Name = petDTO.Name;
                pet.Breed = petDTO.Breed;
                pet.Picture = petDTO.Picture;
                pet.ApartmentId = petDTO.ApartmentId;

                if (!string.IsNullOrWhiteSpace(pet.Picture) && !string.IsNullOrWhiteSpace(petDTO.Picture))
                {
                    await _fileStorage.RemoveFileAsync(pet.Picture, "pets");
                    var photo = Convert.FromBase64String(petDTO.Picture);
                    pet.Picture = await _fileStorage.SaveFileAsync(photo, ".jpg", "pets");
                }
                else if (string.IsNullOrWhiteSpace(pet.Picture) && !string.IsNullOrWhiteSpace(petDTO.Picture))
                {
                    var photo = Convert.FromBase64String(petDTO.Picture);
                    pet.Picture = await _fileStorage.SaveFileAsync(photo, ".jpg", "pets");
                }

                _context.Update(pet);
                await _context.SaveChangesAsync();
                return new ActionResponse<Pet>
                {
                    WasSuccess = true,
                    Result = pet
                };
            }
            catch (DbUpdateException)
            {
                return new ActionResponse<Pet>
                {
                    WasSuccess = false,
                    Message = "Ya existe una mascota con el mismo nombre."
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<Pet>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };
            }
        }
    }
}