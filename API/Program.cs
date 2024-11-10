using Microsoft.EntityFrameworkCore;
using Application.Services;
using Infrastructure.Repositories;
using Infrastructure;
using Domain.Interfaces;
using Application.Interfaces;
using Infrastructure.Caching;
using Infrastructure.ExternalAPIs;

var builder = WebApplication.CreateBuilder(args);

// Configure services for Dependency Injection
builder.Services.AddControllers();

// Add Memory Cache
builder.Services.AddMemoryCache();

// Set up the database connection (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("Infrastructure")));

// Add repositories and services for Dependency Injection
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryCache, CountryCache>();
builder.Services.AddScoped<ICountryApiService, CountryApiService>();
builder.Services.AddScoped<CountriesService>();

// Register HTTP client for the external RESTCountries API
builder.Services.AddHttpClient("RestCountries", client =>
{
    client.BaseAddress = new Uri("https://restcountries.com/v3.1/");
});

// Add Swagger for API documentation (optional but useful)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
