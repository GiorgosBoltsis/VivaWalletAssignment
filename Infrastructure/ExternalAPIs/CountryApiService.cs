using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using System.Text.Json;

namespace Infrastructure.ExternalAPIs
{
    public class CountryApiService : ICountryApiService
    {
        public const string RestCountriesUrl = "https://restcountries.com/v3.1/all";

        private readonly HttpClient _httpClient;

        public CountryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {

            try
            {
                var response = await _httpClient.GetStringAsync(RestCountriesUrl);
                var countries = JsonSerializer.Deserialize<IEnumerable<ExternalCountryDto>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if(countries is null)
                {
                    return Enumerable.Empty<Country>();
                }
                return countries.Select(c => new Country
                {
                    CommonName = c.Name.Common,
                    Capital = string.Join(",", c.Capital ?? new List<string> { "N/A" }),
                    Borders = string.Join(",", c.Borders ?? new List<string> { "N/A" })
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new CountriesExternalApiException(ex.Message);
            }
        }
    }
}
