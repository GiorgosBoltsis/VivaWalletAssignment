using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Tests.UnitTests
{
    public class CountryServiceCachingTests
    {
        private readonly IMemoryCache memoryCache;
        private readonly ICountryCache cache;
        private readonly Mock<ICountryRepository> _mockedCountryRepository;
        private readonly Mock<ICountryApiService> _mockedCountryApiService;
        private readonly CountriesService _countriesService;

        public CountryServiceCachingTests()
        {
            _mockedCountryRepository = new Mock<ICountryRepository>();
            cache = new CountryCache(new MemoryCache(new MemoryCacheOptions()));
            _mockedCountryApiService = new Mock<ICountryApiService>();
            _countriesService = new CountriesService(_mockedCountryRepository.Object, cache, _mockedCountryApiService.Object);
        }

        [Fact]
        public async Task GetCountries_ShouldUseCache_WhenCountriesAreCached()
        {
            // Arrange
            var countries = new List<Country> { new Country { CommonName = "Country1", Capital = "Capital1", Borders = "Border1, Border2" } };


            _mockedCountryApiService.Setup(c => c.GetAllCountriesAsync()).ReturnsAsync(countries);

            var result1 = await _countriesService.GetCountriesAsync();

            var result2 = await _countriesService.GetCountriesAsync();

            Assert.Same(result1.ToString(), result2.ToString());
        }
    }
}
