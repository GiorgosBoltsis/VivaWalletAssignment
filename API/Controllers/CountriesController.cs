using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly CountriesService _countriesService;

        public CountryController(CountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        /// <summary>
        /// Retrieves all the countries.
        /// Steps:
        /// - Check if countries exists in the local cache
        /// - Check if countries exists in the database
        /// - Retrieve all the countries via http call and save them in db and in cache
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _countriesService.GetCountriesAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }
    }
}
