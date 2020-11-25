using Microsoft.AspNetCore.Mvc;
using ShoppingList.Api.ApplicationServices;
using ShoppingList.Api.Contracts.Commands.ChangeItem;
using ShoppingList.Api.Contracts.Commands.CreateItem;
using ShoppingList.Api.Contracts.Commands.CreateTemporaryItem;
using ShoppingList.Api.Contracts.Commands.MakeTemporaryItemPermanent;
using ShoppingList.Api.Contracts.Commands.UpdateItem;
using ShoppingList.Api.Domain.Commands.ChangeItem;
using ShoppingList.Api.Domain.Commands.CreateItem;
using ShoppingList.Api.Domain.Commands.CreateTemporaryItem;
using ShoppingList.Api.Domain.Commands.DeleteItem;
using ShoppingList.Api.Domain.Commands.MakeTemporaryItemPermanent;
using ShoppingList.Api.Domain.Commands.UpdateItem;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.ItemById;
using ShoppingList.Api.Domain.Queries.ItemFilterResults;
using ShoppingList.Api.Domain.Queries.ItemSearch;
using ShoppingList.Api.Domain.Queries.SharedModels;
using ShoppingList.Api.Endpoint.Converters.Item;
using ShoppingList.Api.Endpoint.Extensions.Item;
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
        [Route("change")]
        public async Task<IActionResult> ChangeItem([FromBody] ChangeItemContract changeItemContract)
        {
            var model = changeItemContract.ToDomain();
            var command = new ChangeItemCommand(model);

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (TemporaryItemNotModifyableException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("update")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemContract updateItemContract)
        {
            var model = updateItemContract.ToItemUpdate();
            var command = new UpdateItemCommand(model);

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (TemporaryItemNotUpdateableException e)
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

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("delete/{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute(Name = "itemId")] int itemId)
        {
            var command = new DeleteItemCommand(new StoreItemId(itemId));
            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("{itemId}")]
        public async Task<IActionResult> Get([FromRoute(Name = "itemId")] int itemId)
        {
            var query = new ItemByIdQuery(new StoreItemId(itemId));
            StoreItemReadModel result;
            try
            {
                result = await queryDispatcher.DispatchAsync(query, default);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result.ToContract());
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create/temporary")]
        public async Task<IActionResult> CreateTemporaryItem([FromBody] CreateTemporaryItemContract contract)
        {
            var command = new CreateTemporaryItemCommand(contract.ToDomain());
            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("make-temporary-item-permanent")]
        public async Task<IActionResult> MakeTemporaryItemPermanent([FromBody] MakeTemporaryItemPermanentContract contract)
        {
            var command = new MakeTemporaryItemPermanentCommand(contract.ToDomain());
            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemCategoryNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (ManufacturerNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (ItemIsNotTemporaryException e)
            {
                return BadRequest(e.Message);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}