using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Domain.Commands.AddItemToShoppingList;
using ShoppingList.Domain.Commands.RemoveItemFromShoppingList;
using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.ActiveShoppingListByStoreId;
using ShoppingList.Domain.Queries.AllActiveStores;
using ShoppingList.Domain.Queries.ItemCategorySearch;
using ShoppingList.Domain.Queries.ManufacturerSearch;
using ShoppingList.Domain.Queries.SharedModels;
using ShoppingList.Endpoint.Converters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Endpoint.V1.Controllers
{
    [ApiController]
    [Route("v1/shopping-list")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public ShoppingListController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("shopping-list/active/{storeId}")]
        public async Task<IActionResult> GetActiveShoppingListByStoreId([FromRoute(Name = "storeId")] int storeId)
        {
            var query = new ActiveShoppingListByStoreIdQuery(new StoreId(storeId));
            ShoppingListReadModel readModel;
            try
            {
                readModel = await queryDispatcher.DispatchAsync(query, default);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            var contract = readModel.ToContract();

            return Ok(contract);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("shopping-list/{shoppingListId}/items/{itemId}/remove")]
        public async Task<IActionResult> RemoveItemFromShoppingList(
            [FromRoute(Name = "shoppingListId")] int shoppingListId, [FromRoute(Name = "itemId")] int itemId)
        {
            var command = new RemoveItemFromShoppingListCommand(
                new ShoppingListId(shoppingListId), new ShoppingListItemId(itemId));

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemNotOnShoppingListException e)
            {
                return BadRequest(e.Message);
            }
            catch (ShoppingListNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("shopping-list/{shoppingListId}/items/{itemId}/add/{quantity}")]
        public async Task<IActionResult> AddItemToShoppingList(
            [FromRoute(Name = "shoppingListId")] int shoppingListId, [FromRoute(Name = "itemId")] int itemId,
            [FromRoute(Name = "quantity")] float quantity)
        {
            var command = new AddItemToShoppingListCommand(
                new ShoppingListId(shoppingListId), new ShoppingListItemId(itemId), quantity);

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemAlreadyOnShoppingListException e)
            {
                return BadRequest(e.Message);
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (ShoppingListNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("stores/active")]
        public async Task<IActionResult> GetAllActiveStores()
        {
            var query = new AllActiveStoresQuery();
            var storeReadModels = await queryDispatcher.DispatchAsync(query, default);
            var storeContracts = storeReadModels.Select(store => store.ToContract());

            return Ok(storeContracts);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("item-categories/{search-input}")]
        public async Task<IActionResult> GetItemCategorySearchResults([FromRoute(Name = "search-input")] string searchInput)
        {
            searchInput = searchInput.Trim();
            if (string.IsNullOrEmpty(searchInput))
            {
                return BadRequest("Search input mustn't be null or empty");
            }

            var query = new ItemCategorySearchQuery(searchInput);
            var itemCategoryReadModels = await queryDispatcher.DispatchAsync(query, default);
            var itemCategoryContracts = itemCategoryReadModels.Select(category => category.ToContract());

            return Ok(itemCategoryContracts);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("manufacturer/{search-input}")]
        public async Task<IActionResult> GetManufacturerSearchResults([FromRoute(Name = "search-input")] string searchInput)
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