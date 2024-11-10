using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICountryCache
    {
        IEnumerable<Country> GetCountries();
        void Save(IEnumerable<Country> countries);
    }
}
