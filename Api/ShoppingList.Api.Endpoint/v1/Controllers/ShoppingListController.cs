using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.CreateShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers
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
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
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
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
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
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
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
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
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
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
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
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
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
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
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
        [Route("quantity-types-in-packet")]
        public async Task<IActionResult> GetAllQuantityTypesInPacket()
        {
            var query = new AllQuantityTypesInPacketQuery();
            var readModels = await queryDispatcher.DispatchAsync(query, default);
            var contracts = readModels.Select(rm => rm.ToContract());

            return Ok(contracts);
        }
    }
}