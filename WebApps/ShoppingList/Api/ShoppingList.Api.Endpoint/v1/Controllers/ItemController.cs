using Microsoft.AspNetCore.Mvc;
using ShoppingList.Api.ApplicationServices;
using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Contracts.Commands.UpdateItem;
using ShoppingList.Api.Domain.Commands.CreateItem;
using ShoppingList.Api.Domain.Commands.UpdateItem;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.ItemFilterResults;
using ShoppingList.Api.Domain.Queries.ItemSearch;
using ShoppingList.Api.Endpoint.Converters;
using ShoppingList.Api.Endpoint.Converters.Item;
using ShoppingList.Api.Endpoint.Converters.Store;
using ShoppingList.Endpoint.Converters.Item;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Api.Endpoint.v1.Controllers
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
        [Route("search/{searchInput}/{storeId}")]
        public async Task<IActionResult> GetItemSearchResults([FromRoute(Name = "searchInput")] string searchInput,
            [FromRoute(Name = "storeId")] int storeId)
        {
            var query = new ItemSearchQuery(searchInput, new StoreId(storeId));
            IEnumerable<ItemSearchReadModel> readModels = await queryDispatcher.DispatchAsync(query, default);

            var contracts = readModels.Select(rm => rm.ToContract());

            return Ok(contracts);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("filter")]
        public async Task<IActionResult> GetItemFilterResults([FromQuery] IEnumerable<int> storeIds,
            [FromQuery] IEnumerable<int> itemCategoryIds,
            [FromQuery] IEnumerable<int> manufacturerIds)
        {
            var query = new ItemFilterResultsQuery(
                storeIds.Select(id => new StoreId(id)),
                itemCategoryIds.Select(id => new ItemCategoryId(id)),
                manufacturerIds.Select(id => new ManufacturerId(id)));

            var readModels = await queryDispatcher.DispatchAsync(query, default);

            return Ok(readModels.Select(readModel => readModel.ToContract()));
        }
    }
}