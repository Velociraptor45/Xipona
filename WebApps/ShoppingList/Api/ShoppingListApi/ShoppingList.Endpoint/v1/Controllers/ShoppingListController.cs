using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.ActiveShoppingListByStoreId;
using ShoppingList.Domain.Queries.AllActiveStores;
using ShoppingList.Domain.Queries.ItemCategorySearch;
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

        public ShoppingListController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("active-shopping-list/{storeId}")]
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
    }
}