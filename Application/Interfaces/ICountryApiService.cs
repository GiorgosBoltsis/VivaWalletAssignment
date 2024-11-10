using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICountryApiService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
    }

}
