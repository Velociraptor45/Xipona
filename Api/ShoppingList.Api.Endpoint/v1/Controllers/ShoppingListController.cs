﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.FinishShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
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
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/shopping-lists")]
public class ShoppingListController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IToContractConverter<ShoppingListReadModel, ShoppingListContract> _shoppingListToContractConverter;
    private readonly IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> _quantityTypeToContractConverter;
    private readonly IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> _quantityTypeInPacketToContractConverter;
    private readonly IToDomainConverter<ItemIdContract, OfflineTolerantItemId> _offlineTolerantItemIdConverter;
    private readonly IEndpointConverters _converters;

    public ShoppingListController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IToContractConverter<ShoppingListReadModel, ShoppingListContract> shoppingListToContractConverter,
        IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> quantityTypeToContractConverter,
        IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> quantityTypeInPacketToContractConverter,
        IToDomainConverter<ItemIdContract, OfflineTolerantItemId> offlineTolerantItemIdConverter,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _shoppingListToContractConverter = shoppingListToContractConverter;
        _quantityTypeToContractConverter = quantityTypeToContractConverter;
        _quantityTypeInPacketToContractConverter = quantityTypeInPacketToContractConverter;
        _offlineTolerantItemIdConverter = offlineTolerantItemIdConverter;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ShoppingListContract), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("active/{storeId:guid}")]
    public async Task<IActionResult> GetActiveShoppingListByStoreIdAsync([FromRoute] Guid storeId)
    {
        var query = new ActiveShoppingListByStoreIdQuery(new StoreId(storeId));

        ShoppingListReadModel readModel;
        try
        {
            readModel = await _queryDispatcher.DispatchAsync(query, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        var contract = _shoppingListToContractConverter.ToContract(readModel);

        return Ok(contract);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{shoppingListId:guid}/items")]
    public async Task<IActionResult> RemoveItemFromShoppingListAsync([FromRoute] Guid shoppingListId,
        [FromBody] RemoveItemFromShoppingListContract contract)
    {
        OfflineTolerantItemId tolerantItemId;
        try
        {
            tolerantItemId = _offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;

        var command = new RemoveItemFromShoppingListCommand(
            new ShoppingListId(shoppingListId),
            tolerantItemId,
            itemTypeId);

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound or ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
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
            itemId = _offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var command = new AddItemToShoppingListCommand(
            new ShoppingListId(contract.ShoppingListId),
            itemId,
            contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
            new QuantityInBasket(contract.Quantity));

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
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
            await _commandDispatcher.DispatchAsync(command, default);
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
            ? OfflineTolerantItemId.FromActualId(contract.ItemId.Actual.Value)
            : OfflineTolerantItemId.FromOfflineId(contract.ItemId.Offline!.Value);

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new PutItemInBasketCommand(new ShoppingListId(contract.ShoppingListId), itemId,
            itemTypeId);

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
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
            itemId = _offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
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
            await _commandDispatcher.DispatchAsync(command, default);
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
            itemId = _offlineTolerantItemIdConverter.ToDomain(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new ChangeItemQuantityOnShoppingListCommand(new ShoppingListId(contract.ShoppingListId),
            itemId, itemTypeId, new QuantityInBasket(contract.Quantity));

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
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
    public async Task<IActionResult> FinishList([FromRoute(Name = "shoppingListId")] Guid shoppingListId)
    {
        var command = new FinishShoppingListCommand(new ShoppingListId(shoppingListId), DateTime.UtcNow);
        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
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
        var readModels = await _queryDispatcher.DispatchAsync(query, default);
        var contracts = _quantityTypeToContractConverter.ToContract(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route("quantity-types-in-packet")]
    public async Task<IActionResult> GetAllQuantityTypesInPacket()
    {
        var query = new AllQuantityTypesInPacketQuery();
        var readModels = await _queryDispatcher.DispatchAsync(query, default);
        var contracts = _quantityTypeInPacketToContractConverter.ToContract(readModels);

        return Ok(contracts);
    }
}