using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.FinishShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;
using System.Threading;
using AddItemToShoppingListContract = ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract;

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
    public async Task<IActionResult> GetActiveShoppingListByStoreIdAsync([FromRoute] Guid storeId,
        CancellationToken cancellationToken = default)
    {
        var query = new ActiveShoppingListByStoreIdQuery(new StoreId(storeId));

        ShoppingListReadModel readModel;
        try
        {
            readModel = await _queryDispatcher.DispatchAsync(query, cancellationToken);
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
        [FromBody] RemoveItemFromShoppingListContract contract, CancellationToken cancellationToken = default)
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
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items")]
    public async Task<IActionResult> AddTemporaryItemToShoppingListAsync([FromRoute] Guid id,
        [FromBody] AddTemporaryItemToShoppingListContract contract, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(contract.ItemName))
        {
            return BadRequest("Item name mustn't be empty");
        }

        var command = _converters
            .ToDomain<(Guid, AddTemporaryItemToShoppingListContract), AddTemporaryItemToShoppingListCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/items")]
    public async Task<IActionResult> AddItemToShoppingListAsync([FromRoute] Guid id,
        [FromBody] AddItemToShoppingListContract contract, CancellationToken cancellationToken = default)
    {
        var command = new AddItemToShoppingListCommand(
            new ShoppingListId(id),
            new ItemId(contract.ItemId),
            contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
            new QuantityInBasket(contract.Quantity));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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
        [FromRoute] Guid itemId, [FromRoute] Guid itemTypeId, [FromBody] AddItemWithTypeToShoppingListContract contract,
        CancellationToken cancellationToken = default)
    {
        var command = new AddItemWithTypeToShoppingListCommand(
            new ShoppingListId(id),
            new ItemId(itemId),
            new ItemTypeId(itemTypeId),
            contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
            new QuantityInBasket(contract.Quantity));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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
    [Route("add-items-to-shopping-lists")]
    public async Task<IActionResult> AddItemsToShoppingListsAsync(
        [FromBody] AddItemsToShoppingListsContract contract, CancellationToken cancellationToken = default)
    {
        var command = _converters.ToDomain<AddItemsToShoppingListsContract, AddItemsToShoppingListsCommand>(contract);

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            if (e.Reason.ErrorCode is ErrorReasonCode.StoreNotFound or ErrorReasonCode.ItemNotFound)
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
        [FromBody] PutItemInBasketContract contract, CancellationToken cancellationToken = default)
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
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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
        [FromBody] RemoveItemFromBasketContract contract, CancellationToken cancellationToken = default)
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
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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
        [FromBody] ChangeItemQuantityOnShoppingListContract contract, CancellationToken cancellationToken = default)
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
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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
    public async Task<IActionResult> FinishListAsync([FromRoute] Guid id, [FromQuery] DateTimeOffset? finishedAt,
        CancellationToken cancellationToken = default)
    {
        var command = new FinishShoppingListCommand(new ShoppingListId(id), finishedAt ?? DateTimeOffset.UtcNow);
        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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