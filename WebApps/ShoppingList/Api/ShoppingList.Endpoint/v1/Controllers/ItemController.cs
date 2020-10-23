using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Contracts.Commands.CreateItem;
using ShoppingList.Contracts.Commands.UpdateItem;
using ShoppingList.Domain.Commands.CreateItem;
using ShoppingList.Domain.Commands.UpdateItem;
using ShoppingList.Domain.Exceptions;
using ShoppingList.Endpoint.Converters;
using ShoppingList.Endpoint.Converters.Store;
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
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemContract contract)
        {
            var model = contract.ToDomain();
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
    }
}