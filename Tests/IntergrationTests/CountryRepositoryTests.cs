using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Infrastructure.Repositories;
using Domain.Entities;

namespace Tests.IntergrationTests
{
    public class CountryRepositoryTests
    {
        private readonly ApplicationDbContext _context;

        public CountryRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);

            var countryRepository = new CountryRepository(_context);
        }

        [Fact]
        public async Task SaveCountriesToDatabase_ShouldSaveCountriesSuccessfully()
        {
            // Act
            var countries = new List<Country> { new Country { CommonName = "Country1", Capital = "Capital1", Borders = "Border1, Border2" } };

            // Save to database
            _context.Countries.AddRange(countries);
            await _context.SaveChangesAsync();

            // Assert
            var savedCountries = _context.Countries.ToList();
            Assert.NotEmpty(savedCountries);
            Assert.Equal(countries.Count(), savedCountries.Count);
        }
    }
}
