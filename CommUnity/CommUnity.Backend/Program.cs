using CommUnity.BackEnd.Data;
using CommUnity.BackEnd.Repositories.Implementations;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.Services;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x =>  x.UseSqlServer("name=LocalConnection"));
builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IApiService, ApiService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));

builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IStatesRepository, StatesRepository>();
builder.Services.AddScoped<IResidentialUnitRepository, ResidentialUnitRepository>();
builder.Services.AddScoped<IApartmentsRepository, ApartmentsRepository>();
builder.Services.AddScoped<ICommonZonesRepository, CommonZonesRepository>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IPetsRepository, PetsRepository>();
builder.Services.AddScoped<IVehiclesRepository, VehiclesRepository>();


builder.Services.AddScoped<ICitiesUnitOfWork, CitiesUnitOfWork>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();
builder.Services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();
builder.Services.AddScoped<IResidentialUnitUnitOfWork, ResidentialUnitUnitOfWork>();
builder.Services.AddScoped<IApartmentsUnitOfWork, ApartmentsUnitOfWork>();
builder.Services.AddScoped<ICommonZonesUnitOfWork, CommonZonesUnitOfWork>();
builder.Services.AddScoped<INewsUnitOfWork, NewsUnitOfWork>();
builder.Services.AddScoped<IPetsUnitOfWork,PetsUnitOfWork>();
builder.Services.AddScoped<IVehiclesUnitOfWork, VehiclesUnitOfWork>();

var app = builder.Build();

SeedData(app);

void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();