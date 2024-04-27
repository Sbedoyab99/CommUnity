using CommUnity.BackEnd.Services;
using CommUnity.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommUnity.BackEnd.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;

        public SeedDb(DataContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckResidentialUnitsAsync();
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
    }
}