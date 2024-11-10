using Infrastructure.ExternalAPIs;
using Xunit;

namespace Tests.UnitTests
{
    public class CountryApiServiceIntegrationTests
    {
        private readonly HttpClient _httpClient;
        private readonly CountryApiService _countryApiService;

        public CountryApiServiceIntegrationTests()
        {
            _httpClient = new HttpClient();
            _countryApiService = new CountryApiService(_httpClient);
        }

       [Fact]
        public async Task GetCountries_ShouldReturnCountries_WhenApiResponseIsSuccessful()
        {
            // Act
            var result = await _countryApiService.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
