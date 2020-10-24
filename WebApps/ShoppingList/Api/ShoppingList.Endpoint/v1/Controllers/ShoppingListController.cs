using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Domain.Commands.AddItemToShoppingList;
using ShoppingList.Domain.Commands.CreateShoppingList;
using ShoppingList.Domain.Commands.FinishShoppingList;
using ShoppingList.Domain.Commands.RemoveItemFromShoppingList;
using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.ActiveShoppingListByStoreId;
using ShoppingList.Domain.Queries.SharedModels;
using ShoppingList.Endpoint.Converters;
using System;
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
        [Route("active/{storeId}")]
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
        [Route("{shoppingListId}/items/{itemId}/remove")]
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
        [Route("{shoppingListId}/items/{itemId}/add/{quantity}")]
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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("create/{storeId}")]
        public async Task<IActionResult> CreatList([FromRoute(Name = "storeId")] int storeId)
        {
            var command = new CreateShoppingListCommand(new StoreId(storeId));

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ShoppingListAlreadyExistsException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("{shoppingListId}/finish")]
        public async Task<IActionResult> FinishList([FromRoute(Name = "shoppingListId")] int shoppingListId)
        {
            var command = new FinishShoppingListCommand(new ShoppingListId(shoppingListId), DateTime.Now);
            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ShoppingListNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}