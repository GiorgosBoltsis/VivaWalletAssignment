using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching
{
    public class CountryCache : ICountryCache
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "CountiesCache";

        public CountryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<Country> GetCountries()
        {
            return _cache.TryGetValue(CacheKey, out IEnumerable<Country> countries)
                ? countries
                : Enumerable.Empty<Country>();
        }

        public void Save(IEnumerable<Country> countries)
        {
            _cache.Set(CacheKey, countries, TimeSpan.FromHours(1));
        }
    }
}
