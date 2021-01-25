using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ItemCategory;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers
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
        [ProducesResponseType(200)]
        [Route("create/{name}")]
        public async Task<IActionResult> CreateItemCategory([FromRoute(Name = "name")] string name)
        {
            var command = new CreateItemCategoryCommand(name);
            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("delete")]
        public async Task<IActionResult> DeleteItemCategory([FromBody] DeleteItemCategoryContract contract)
        {
            var command = new DeleteItemCategoryCommand(new ItemCategoryId(contract.ItemCategoryId));
            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
            }

            return Ok();
        }
    }
}