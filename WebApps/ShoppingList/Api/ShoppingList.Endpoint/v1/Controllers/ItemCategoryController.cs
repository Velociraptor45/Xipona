using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Domain.Queries.ItemCategorySearch;
using ShoppingList.Endpoint.Converters;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Endpoint.v1.Controllers
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
    }
}