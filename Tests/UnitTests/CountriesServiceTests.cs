using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System.Net;
using Xunit;

namespace Tests.UnitTests
{
    public class CountriesServiceTests
    {
        private readonly Mock<ICountryRepository> _mockedCountryRepository;
        private readonly Mock<ICountryCache> _mockedCountryCache;
        private readonly Mock<ICountryApiService> _mockedCountryApiService;
        private readonly CountriesService _countriesService;


        public CountriesServiceTests()
        {
            _mockedCountryRepository = new Mock<ICountryRepository>();
            _mockedCountryCache = new Mock<ICountryCache>();
            _mockedCountryApiService = new Mock<ICountryApiService>();
            _countriesService = new CountriesService(_mockedCountryRepository.Object, _mockedCountryCache.Object, _mockedCountryApiService.Object);
        }

        [Fact]
        public async Task GetCountries_ShouldReturnCountries_FromCache()
        {
            // Arrange
            var countries = new List<Country> { new Country { CommonName = "Country1", Capital = "Capital1", Borders = "Border1, Border2" } };
            _mockedCountryCache.Setup(m => m.GetCountries()).Returns(countries);

            // Act
            List<CountryDto> result = (List<CountryDto>)await _countriesService.GetCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal("Country1", result[0].CommonName);
            Assert.Equal("Capital1", result[0].Capital);
            Assert.Equal("Border1, Border2", result[0].Borders);
        }
        [Fact]
        public async Task GetCountries_ShouldReturnCountries_FromDatabase_WhenCacheIsEmpty()
        {
            // Arrange
            var countries = new List<Country> { new Country { CommonName = "Country1", Capital = "Capital1", Borders = "Border1, Border2" } };
            _mockedCountryCache.Setup(m => m.GetCountries()).Returns(Enumerable.Empty<Country>());
            _mockedCountryRepository.Setup(r => r.GetCountriesAsync()).ReturnsAsync(countries);

            // Act
            List<CountryDto> result = (List<CountryDto>)await _countriesService.GetCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal("Country1", result[0].CommonName);
            Assert.Equal("Capital1", result[0].Capital);
            Assert.Equal("Border1, Border2", result[0].Borders);
        }

        [Fact]
        public async Task GetCountries_ShouldReturnCountries_FromApi_WhenDatabaseIsEmpty()
        {
            // Arrange
            var countries = new List<Country> { new Country { CommonName = "Country1", Capital = "Capital1", Borders = "Border1, Border2" } };
            _mockedCountryCache.Setup(m => m.GetCountries()).Returns(Enumerable.Empty<Country>());
            _mockedCountryRepository.Setup(r => r.GetCountriesAsync()).ReturnsAsync(new List<Country>());
            _mockedCountryApiService.Setup(a => a.GetAllCountriesAsync()).ReturnsAsync(countries);

            // Act
            List<CountryDto> result = (List<CountryDto>)await _countriesService.GetCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal("Country1", result[0].CommonName);
            Assert.Equal("Capital1", result[0].Capital);
            Assert.Equal("Border1, Border2", result[0].Borders);
        }
    }
}