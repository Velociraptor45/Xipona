using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Domain.Queries.ManufacturerSearch;
using ShoppingList.Endpoint.Converters;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Endpoint.v1.Controllers
{
    [ApiController]
    [Route("v1/manufacturer")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public ManufacturerController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("search/{searchInput}")]
        public async Task<IActionResult> GetManufacturerSearchResults([FromRoute(Name = "searchInput")] string searchInput)
        {
            searchInput = searchInput.Trim();
            if (string.IsNullOrEmpty(searchInput))
            {
                return BadRequest("Search input mustn't be null or empty");
            }

            var query = new ManufacturerSearchQuery(searchInput);
            var manufacturerReadModels = await queryDispatcher.DispatchAsync(query, default);
            var manufacturerContracts = manufacturerReadModels.Select(category => category.ToContract());

            return Ok(manufacturerContracts);
        }
    }
}