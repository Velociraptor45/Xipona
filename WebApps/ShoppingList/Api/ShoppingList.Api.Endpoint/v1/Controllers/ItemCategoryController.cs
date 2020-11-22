using Microsoft.AspNetCore.Mvc;
using ShoppingList.Api.ApplicationServices;
using ShoppingList.Api.Domain.Commands.CreateItemCategory;
using ShoppingList.Api.Domain.Queries.AllActiveItemCategories;
using ShoppingList.Api.Domain.Queries.ItemCategorySearch;
using ShoppingList.Api.Endpoint.Extensions.ItemCategory;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Api.Endpoint.v1.Controllers
{
    [ApiController]
    [Route("v1/item-category")]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public ItemCategoryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("search/{searchInput}")]
        public async Task<IActionResult> GetItemCategorySearchResults([FromRoute(Name = "searchInput")] string searchInput)
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
        [Route("all/active")]
        public async Task<IActionResult> GetAllActiveItemCategories()
        {
            var query = new AllActiveItemCategoriesQuery();
            var readModels = await queryDispatcher.DispatchAsync(query, default);
            var contracts = readModels.Select(readModel => readModel.ToActiveContract());

            return Ok(contracts);
        }

        [HttpPost]
        [ProducesResponseType(500)]
        [Route("create/{name}")]
        public async Task<IActionResult> CreateItemCategory([FromRoute(Name = "name")] string name)
        {
            var command = new CreateItemCategoryCommand(name);
            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }
    }
}