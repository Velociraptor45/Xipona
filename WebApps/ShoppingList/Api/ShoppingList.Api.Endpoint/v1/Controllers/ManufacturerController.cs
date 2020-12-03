using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.ManufacturerSearch;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Manufacturer;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers
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

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("all/active")]
        public async Task<IActionResult> GetAllActiveManufacturers()
        {
            var query = new AllActiveManufacturersQuery();
            var readModels = await queryDispatcher.DispatchAsync(query, default);
            var contracts = readModels.Select(readModel => readModel.ToActiveContract());

            return Ok(contracts);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create/{name}")]
        public async Task<IActionResult> CreateManufacturer([FromRoute(Name = "name")] string name)
        {
            var command = new CreateManufacturerCommand(name);
            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }
    }
}