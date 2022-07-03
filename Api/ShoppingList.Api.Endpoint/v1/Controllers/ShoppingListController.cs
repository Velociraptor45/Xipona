using Microsoft.AspNetCore.Http;
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
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/shopping-lists")]
public class ShoppingListController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public ShoppingListController(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
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

        var contract = _converters.ToContract<ShoppingListReadModel, ShoppingListContract>(readModel);

        return Ok(contract);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items")]
    public async Task<IActionResult> RemoveItemFromShoppingListAsync([FromRoute] Guid id,
        [FromBody] RemoveItemFromShoppingListContract contract)
    {
        OfflineTolerantItemId tolerantItemId;
        try
        {
            tolerantItemId = _converters.ToDomain<ItemIdContract, OfflineTolerantItemId>(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;

        var command = new RemoveItemFromShoppingListCommand(
            new ShoppingListId(id),
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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items")]
    public async Task<IActionResult> AddItemToShoppingListAsync([FromRoute] Guid id,
        [FromBody] AddItemToShoppingListContract contract)
    {
        OfflineTolerantItemId itemId;
        try
        {
            itemId = _converters.ToDomain<ItemIdContract, OfflineTolerantItemId>(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var command = new AddItemToShoppingListCommand(
            new ShoppingListId(id),
            itemId,
            contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
            new QuantityInBasket(contract.Quantity));

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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items/{itemId:guid}/{itemTypeId:guid}")]
    public async Task<IActionResult> AddItemWithTypeToShoppingListAsync([FromRoute] Guid id,
        [FromRoute] Guid itemId, [FromRoute] Guid itemTypeId, [FromBody] AddItemWithTypeToShoppingListContract contract)
    {
        var command = new AddItemWithTypeToShoppingListCommand(
            new ShoppingListId(id),
            new ItemId(itemId),
            new ItemTypeId(itemTypeId),
            contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
            new QuantityInBasket(contract.Quantity));

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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items/basket/add")]
    public async Task<IActionResult> PutItemInBasketAsync([FromRoute] Guid id,
        [FromBody] PutItemInBasketContract contract)
    {
        OfflineTolerantItemId itemId;
        try
        {
            itemId = _converters.ToDomain<ItemIdContract, OfflineTolerantItemId>(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new PutItemInBasketCommand(new ShoppingListId(id), itemId, itemTypeId);

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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items/basket/remove")]
    public async Task<IActionResult> RemoveItemFromBasketAsync([FromRoute] Guid id,
        [FromBody] RemoveItemFromBasketContract contract)
    {
        OfflineTolerantItemId itemId;
        try
        {
            itemId = _converters.ToDomain<ItemIdContract, OfflineTolerantItemId>(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new RemoveItemFromBasketCommand(new ShoppingListId(id), itemId, itemTypeId);

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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items/quantity")]
    public async Task<IActionResult> ChangeItemQuantityOnShoppingListAsync([FromRoute] Guid id,
        [FromBody] ChangeItemQuantityOnShoppingListContract contract)
    {
        OfflineTolerantItemId itemId;
        try
        {
            itemId = _converters.ToDomain<ItemIdContract, OfflineTolerantItemId>(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new ChangeItemQuantityOnShoppingListCommand(new ShoppingListId(id), itemId,
            itemTypeId, new QuantityInBasket(contract.Quantity));

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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/finish")]
    public async Task<IActionResult> FinishListAsync([FromRoute] Guid id)
    {
        var command = new FinishShoppingListCommand(new ShoppingListId(id), DateTimeOffset.UtcNow);
        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return Ok();
    }
}