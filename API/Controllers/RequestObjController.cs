using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestObjController : ControllerBase
    {

        /// <summary>
        /// Finds the second largest number from the provided list
        /// </summary>
        [HttpPost("second-largest-number")]
        public ActionResult<int> FindSecondLargestNumber([FromBody] RequestObj request)
        {
            if (request?.RequestArrayObj == null || request.RequestArrayObj.Count() < 2)
            {
                return BadRequest("RequestArray should have at least two elements.");
            }

            var distinctArray = request.RequestArrayObj.Distinct().OrderByDescending(x => x).ToList();

            if (distinctArray.Count < 2)
            {
                return BadRequest("RequestArray doesnt have a second unique largest integer. All elements are identical");
            }

            return Ok(distinctArray[1]);
        }
    }

}
