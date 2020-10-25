using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Contracts.Commands.CreateItem;
using ShoppingList.Contracts.Commands.UpdateItem;
using ShoppingList.Domain.Commands.CreateItem;
using ShoppingList.Domain.Commands.UpdateItem;
using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.ItemSearch;
using ShoppingList.Endpoint.Converters;
using ShoppingList.Endpoint.Converters.Item;
using ShoppingList.Endpoint.Converters.Store;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Endpoint.v1.Controllers
{
    [ApiController]
    [Route("v1/item")]
    public class ItemController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public ItemController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemContract createItemContract)
        {
            var model = createItemContract.ToDomain();
            var command = new CreateItemCommand(model);

            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("update")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemContract updateItemContract)
        {
            var model = updateItemContract.ToDomain();
            var command = new UpdateItemCommand(model);

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("search/{search-input}/{storeId}")]
        public async Task<IActionResult> GetItemSearchResults([FromRoute(Name = "search-input")] string searchInput,
            [FromRoute(Name = "storeId")] int storeId)
        {
            var query = new ItemSearchQuery(searchInput, new StoreId(storeId));
            IEnumerable<ItemSearchReadModel> readModels = await queryDispatcher.DispatchAsync(query, default);

            var contracts = readModels.Select(rm => rm.ToContract());

            return Ok(contracts);
        }
    }
}