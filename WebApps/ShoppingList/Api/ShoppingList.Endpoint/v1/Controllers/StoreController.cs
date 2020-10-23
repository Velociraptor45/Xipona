using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Domain.Queries.AllActiveStores;
using ShoppingList.Endpoint.Converters;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Endpoint.v1.Controllers
{
    [ApiController]
    [Route("v1/store")]
    public class StoreController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public StoreController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("active")]
        public async Task<IActionResult> GetAllActiveStores()
        {
            var query = new AllActiveStoresQuery();
            var storeReadModels = await queryDispatcher.DispatchAsync(query, default);
            var storeContracts = storeReadModels.Select(store => store.ToContract());

            return Ok(storeContracts);
        }
    }
}