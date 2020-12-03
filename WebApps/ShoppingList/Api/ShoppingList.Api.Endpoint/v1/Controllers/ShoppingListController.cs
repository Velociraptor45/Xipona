using Microsoft.AspNetCore.Mvc;
using ShoppingList.Api.ApplicationServices;
using ShoppingList.Api.Contracts.Commands.AddItemToShoppingList;
using ShoppingList.Api.Contracts.Commands.ChangeItemQuantityOnShoppingList;
using ShoppingList.Api.Contracts.Commands.PutItemInBasket;
using ShoppingList.Api.Contracts.Commands.RemoveItemFromBasket;
using ShoppingList.Api.Contracts.Commands.RemoveItemFromShoppingList;
using ShoppingList.Api.Domain.Commands.AddItemToShoppingList;
using ShoppingList.Api.Domain.Commands.ChangeItemQuantityOnShoppingList;
using ShoppingList.Api.Domain.Commands.CreateShoppingList;
using ShoppingList.Api.Domain.Commands.FinishShoppingList;
using ShoppingList.Api.Domain.Commands.PutItemInBasket;
using ShoppingList.Api.Domain.Commands.RemoveItemFromBasket;
using ShoppingList.Api.Domain.Commands.RemoveItemFromShoppingList;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.ActiveShoppingListByStoreId;
using ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;
using ShoppingList.Api.Domain.Queries.SharedModels;
using ShoppingList.Api.Endpoint.Extensions.ShoppingList;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Api.Endpoint.v1.Controllers
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
        [Route("is-alive")]
        public IActionResult IsAlive()
        {
            return Ok(true);
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
        [Route("items/remove")]
        public async Task<IActionResult> RemoveItemFromShoppingList(
            [FromBody] RemoveItemFromShoppingListContract contract)
        {
            ShoppingListItemId itemId;
            try
            {
                itemId = contract.ItemId.ToShoppingListItemId();
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var command = new RemoveItemFromShoppingListCommand(new ShoppingListId(contract.ShoppingListId), itemId);

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
        [Route("items/add")]
        public async Task<IActionResult> AddItemToShoppingList([FromBody] AddItemToShoppingListContract contract)
        {
            ShoppingListItemId itemId;
            try
            {
                itemId = contract.ItemId.ToShoppingListItemId();
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var command = new AddItemToShoppingListCommand(
                new ShoppingListId(contract.ShoppingListId), itemId, contract.Quantity);

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
            catch (ItemAtStoreNotAvailableException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("items/put-in-basket")]
        public async Task<IActionResult> PutItemInBasket([FromBody] PutItemInBasketContract contract)
        {
            var command = new PutItemInBasketCommand(new ShoppingListId(contract.ShopingListId),
                new ShoppingListItemId(contract.ItemId.Actual.Value));

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemNotOnShoppingListException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("items/remove-from-basket")]
        public async Task<IActionResult> RemoveItemFromBasket([FromBody] RemoveItemFromBasketContract contract)
        {
            ShoppingListItemId itemId;
            try
            {
                itemId = contract.ItemId.ToShoppingListItemId();
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var command = new RemoveItemFromBasketCommand(new ShoppingListId(contract.ShoppingListId), itemId);
            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemNotOnShoppingListException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("items/change-quantity")]
        public async Task<IActionResult> ChangeItemQuantityOnShoppingList(
            [FromBody] ChangeItemQuantityOnShoppingListContract contract)
        {
            ShoppingListItemId itemId;
            try
            {
                itemId = contract.ItemId.ToShoppingListItemId();
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var command = new ChangeItemQuantityOnShoppingListCommand(new ShoppingListId(contract.ShoppingListId),
                itemId, contract.Quantity);

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (ItemNotOnShoppingListException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("quantity-types")]
        public async Task<IActionResult> GetAllQuantityTypes()
        {
            var query = new AllQuantityTypesQuery();
            var readModels = await queryDispatcher.DispatchAsync(query, default);
            var contracts = readModels.Select(rm => rm.ToContract());

            return Ok(contracts);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("quantity-in-packet-types")]
        public async Task<IActionResult> GetAllQuantityInPacketTypes()
        {
            var query = new AllQuantityInPacketTypesQuery();
            var readModels = await queryDispatcher.DispatchAsync(query, default);
            var contracts = readModels.Select(rm => rm.ToContract());

            return Ok(contracts);
        }
    }
}