using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers
{
    [ApiController]
    [Route("v1/shopping-list")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IToContractConverter<ShoppingListReadModel, ShoppingListContract> shoppingListToContractConverter;
        private readonly IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeToContractConverter;
        private readonly IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketToContractConverter;
        private readonly IToDomainConverter<ItemIdContract, OfflineTolerantItemId> offlineTolerantItemIdConverter;
        private readonly IEndpointConverters _converters;

        public ShoppingListController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
            IToContractConverter<ShoppingListReadModel, ShoppingListContract> shoppingListToContractConverter,
            IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeToContractConverter,
            IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketToContractConverter,
            IToDomainConverter<ItemIdContract, OfflineTolerantItemId> offlineTolerantItemIdConverter,
            IEndpointConverters converters)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.shoppingListToContractConverter = shoppingListToContractConverter;
            this.quantityTypeToContractConverter = quantityTypeToContractConverter;
            this.quantityTypeInPacketToContractConverter = quantityTypeInPacketToContractConverter;
            this.offlineTolerantItemIdConverter = offlineTolerantItemIdConverter;
            _converters = converters;
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

            var contract = shoppingListToContractConverter.ToContract(readModel);

            return Ok(contract);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("items/remove")]
        public async Task<IActionResult> RemoveItemFromShoppingList(
            [FromBody] RemoveItemFromShoppingListContract contract)
        {
            OfflineTolerantItemId itemId;
            try
            {
                itemId = offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var itemTypeId = contract.ItemTypeId.HasValue
                ? new ItemTypeId(contract.ItemTypeId.Value)
                : (ItemTypeId?)null;

            var command = new RemoveItemFromShoppingListCommand(new ShoppingListId(contract.ShoppingListId), itemId,
                itemTypeId);

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
            OfflineTolerantItemId itemId;
            try
            {
                itemId = offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var command = new AddItemToShoppingListCommand(
                new ShoppingListId(contract.ShoppingListId),
                itemId,
                contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
                contract.Quantity);

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
        [Route("items/add-with-type")]
        public async Task<IActionResult> AddItemWithTypeToShoppingList([FromBody]
            AddItemWithTypeToShoppingListContract contract)
        {
            var command = _converters.ToDomain<AddItemWithTypeToShoppingListContract, AddItemWithTypeToShoppingListCommand>(
                contract);

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (DomainException e)
            {
                return UnprocessableEntity(e.Reason);
            }

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("items/put-in-basket")]
        public async Task<IActionResult> PutItemInBasket([FromBody] PutItemInBasketContract contract)
        {
            if (contract.ItemId.Actual is null && contract.ItemId.Offline is null)
                return BadRequest("At least one item id must be specified");

            var itemId = contract.ItemId.Actual != null
                ? new OfflineTolerantItemId(contract.ItemId.Actual.Value)
                : new OfflineTolerantItemId(contract.ItemId.Offline!.Value);

            var itemTypeId = contract.ItemTypeId.HasValue
                ? new ItemTypeId(contract.ItemTypeId.Value)
                : (ItemTypeId?)null;
            var command = new PutItemInBasketCommand(new ShoppingListId(contract.ShoppingListId), itemId,
                itemTypeId);

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
            OfflineTolerantItemId itemId;
            try
            {
                itemId = offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var itemTypeId = contract.ItemTypeId.HasValue
                ? new ItemTypeId(contract.ItemTypeId.Value)
                : (ItemTypeId?)null;
            var command = new RemoveItemFromBasketCommand(new ShoppingListId(contract.ShoppingListId), itemId,
                itemTypeId);

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
            OfflineTolerantItemId itemId;
            try
            {
                itemId = offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
            }
            catch (ArgumentException)
            {
                return BadRequest("No item id was specified.");
            }

            var itemTypeId = contract.ItemTypeId.HasValue
                ? new ItemTypeId(contract.ItemTypeId.Value)
                : (ItemTypeId?)null;
            var command = new ChangeItemQuantityOnShoppingListCommand(new ShoppingListId(contract.ShoppingListId),
                itemId, itemTypeId, contract.Quantity);

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
            var command = new FinishShoppingListCommand(new ShoppingListId(shoppingListId), DateTime.UtcNow);
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
            var contracts = quantityTypeToContractConverter.ToContract(readModels);

            return Ok(contracts);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("quantity-types-in-packet")]
        public async Task<IActionResult> GetAllQuantityTypesInPacket()
        {
            var query = new AllQuantityTypesInPacketQuery();
            var readModels = await queryDispatcher.DispatchAsync(query, default);
            var contracts = quantityTypeInPacketToContractConverter.ToContract(readModels);

            return Ok(contracts);
        }
    }
}