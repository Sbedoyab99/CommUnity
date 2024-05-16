using CommUnity.BackEnd.Services;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public SeedDb(DataContext context, IApiService apiService, IUsersUnitOfWork usersUnitOfWork)
        {
            _context = context;
            _apiService = apiService;
            _usersUnitOfWork = usersUnitOfWork;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckResidentialUnitsAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Luis", "Viana", "viana1217@yopmail.com", "3145588855", "Calle Luna Calle Sol", UserType.Admin);
        }

        private async Task CheckCountriesAsync()
        {
            if (_context.Countries.Any())
            {
                return; // Si ya hay datos, no hagas nada
            }

            var filePath = "Data/CountriesStatesCities.sql";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"No se encontró el archivo SQL en la ruta: {filePath}");
            }

            var sqlContent = File.ReadAllText(filePath);

            try
            {
                await _context.Database.ExecuteSqlRawAsync(sqlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Aquí puedes registrar el error o manejarlo según sea necesario
            }

            await _context.SaveChangesAsync();
        }
        private async Task CheckResidentialUnitsAsync()
        {
            if (_context.ResidentialUnits.Any())
            {
                return; // Si ya hay datos, no hagas nada
            }

            var filePath = "Data/ResidentialUnits.sql";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"No se encontró el archivo SQL en la ruta: {filePath}");
            }

            var sqlContent = File.ReadAllText(filePath);

            try
            {
                await _context.Database.ExecuteSqlRawAsync(sqlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Aquí puedes registrar el error o manejarlo según sea necesario
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.AdminResidentialUnit.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                city ??= await _context.Cities.FirstOrDefaultAsync();

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = city,//_context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

                var token = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
                await _usersUnitOfWork.ConfirmEmailAsync(user, token);

            }

            return user;
        }

    }
}