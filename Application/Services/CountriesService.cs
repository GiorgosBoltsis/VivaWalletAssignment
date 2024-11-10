using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class CountriesService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ICountryCache _countryCache;
        private readonly ICountryApiService _countryApiService;
        public CountriesService(ICountryRepository countryRepository,
                                 ICountryCache countryCache,
                                 ICountryApiService countryApiService)
        {
            _countryRepository = countryRepository;
            _countryCache = countryCache;
            _countryApiService = countryApiService;
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
        {
            var cachedCountries = _countryCache.GetCountries();
            if (cachedCountries.Any()) return ToCountryDtos(cachedCountries);

            var countries = await _countryRepository.GetCountriesAsync();
            if (countries.Any())
            {
                _countryCache.Save(countries);
                return ToCountryDtos(countries);
            }

            var apiCountries = await _countryApiService.GetAllCountriesAsync();
            await _countryRepository.SaveCountriesAsync(apiCountries);
            _countryCache.Save(apiCountries);

            return ToCountryDtos(apiCountries);
        }

        private static IEnumerable<CountryDto> ToCountryDtos(IEnumerable<Country> countries)
        {
            var countriesDto = countries.Select(c => new CountryDto
            {
                CommonName = c.CommonName,
                Capital = c.Capital,
                Borders = c.Borders
            }).ToList();
            return countriesDto;
        }
    }
}
