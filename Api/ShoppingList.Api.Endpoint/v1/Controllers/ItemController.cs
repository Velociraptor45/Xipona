using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.DeleteItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.ItemById;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItems;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItemsByFilters;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/items")]
public class ItemController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public ItemController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _converters = converters;
    }

    [HttpPost]
    [ProducesResponseType(typeof(StoreItemContract), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("without-types")]
    public async Task<IActionResult> CreateItemAsync([FromBody] CreateItemContract createItemContract)
    {
        try
        {
            var model = _converters.ToDomain<CreateItemContract, ItemCreation>(createItemContract);
            var command = new CreateItemCommand(model);

            var readModel = await _commandDispatcher.DispatchAsync(command, default);

            var contract = _converters.ToContract<StoreItemReadModel, StoreItemContract>(readModel);

            return CreatedAtAction(nameof(Get), new { itemId = contract.Id }, contract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(StoreItemContract), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("with-types")]
    public async Task<IActionResult> CreateItemWithTypesAsync([FromBody] CreateItemWithTypesContract contract)
    {
        try
        {
            var model = _converters.ToDomain<CreateItemWithTypesContract, IStoreItem>(contract);
            var command = new CreateItemWithTypesCommand(model);
            var readModel = await _commandDispatcher.DispatchAsync(command, default);

            var returnContract = _converters.ToContract<StoreItemReadModel, StoreItemContract>(readModel);

            return CreatedAtAction(nameof(Get), new { itemId = returnContract.Id }, returnContract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("with-types/{id}/modify")]
    public async Task<IActionResult> ModifyItemWithTypesAsync([FromRoute] Guid id,
        [FromBody] ModifyItemWithTypesContract contract)
    {
        var command =
            _converters.ToDomain<(Guid, ModifyItemWithTypesContract), ModifyItemWithTypesCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("without-types/{id}/modify")]
    public async Task<IActionResult> ModifyItemAsync([FromRoute] Guid id,
        [FromBody] ModifyItemContract contract)
    {
        var command = _converters.ToDomain<(Guid, ModifyItemContract), ModifyItemCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("without-types/{id}/update")]
    public async Task<IActionResult> UpdateItemAsync([FromRoute] Guid id,
        [FromBody] UpdateItemContract contract)
    {
        var command = _converters.ToDomain<(Guid, UpdateItemContract), UpdateItemCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("with-types/{id}/update")]
    public async Task<IActionResult> UpdateItemWithTypesAsync([FromRoute] Guid id,
        [FromBody] UpdateItemWithTypesContract contract)
    {
        var command =
            _converters.ToDomain<(Guid, UpdateItemWithTypesContract), UpdateItemWithTypesCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SearchItemForShoppingListResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("search/{storeId}")]
    public async Task<IActionResult> SearchItemsForShoppingListAsync([FromRoute] Guid storeId,
        [FromQuery] string searchInput)
    {
        var query = new SearchItemsForShoppingListQuery(searchInput, new StoreId(storeId));

        IEnumerable<SearchItemForShoppingResultReadModel> readModels;
        try
        {
            readModels = await _queryDispatcher.DispatchAsync(query, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        if (!readModels.Any())
            return NoContent();

        var contracts =
            _converters.ToContract<SearchItemForShoppingResultReadModel, SearchItemForShoppingListResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SearchItemForShoppingListResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("search")]
    public async Task<IActionResult> SearchItemsAsync([FromQuery] string searchInput)
    {
        var query = new SearchItemQuery(searchInput);

        List<SearchItemResultReadModel> readModels;
        try
        {
            readModels = (await _queryDispatcher.DispatchAsync(query, default)).ToList();
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }

        if (!readModels.Any())
            return NoContent();

        var contracts =
            _converters.ToContract<SearchItemResultReadModel, SearchItemResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route("search-by-filter")]
    public async Task<IActionResult> SearchItemsByFilterAsync([FromQuery] IEnumerable<Guid> storeIds,
        [FromQuery] IEnumerable<Guid> itemCategoryIds,
        [FromQuery] IEnumerable<Guid> manufacturerIds)
    {
        var query = new SearchItemsByFilterQuery(
            storeIds.Select(id => new StoreId(id)),
            itemCategoryIds.Select(id => new ItemCategoryId(id)),
            manufacturerIds.Select(id => new ManufacturerId(id)));

        var readModels = await _queryDispatcher.DispatchAsync(query, default);
        var contracts = _converters.ToContract<SearchItemResultReadModel, SearchItemResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Route("delete/{itemId}")]
    public async Task<IActionResult> DeleteItem([FromRoute(Name = "itemId")] Guid itemId)
    {
        var command = new DeleteItemCommand(new ItemId(itemId));
        await _commandDispatcher.DispatchAsync(command, default);

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("{itemId}")]
    public async Task<IActionResult> Get([FromRoute(Name = "itemId")] Guid itemId)
    {
        var query = new ItemByIdQuery(new ItemId(itemId));
        StoreItemReadModel result;
        try
        {
            result = await _queryDispatcher.DispatchAsync(query, default);
        }
        catch (DomainException e)
        {
            return BadRequest(e.Reason);
        }

        var contract = _converters.ToContract<StoreItemReadModel, StoreItemContract>(result);

        return Ok(contract);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Route("create/temporary")]
    public async Task<IActionResult> CreateTemporaryItem([FromBody] CreateTemporaryItemContract contract)
    {
        var model = _converters.ToDomain<CreateTemporaryItemContract, TemporaryItemCreation>(contract);
        var command = new CreateTemporaryItemCommand(model);
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
    [Route("make-temporary-item-permanent")]
    public async Task<IActionResult> MakeTemporaryItemPermanent([FromBody] MakeTemporaryItemPermanentContract contract)
    {
        var model = _converters.ToDomain<MakeTemporaryItemPermanentContract, PermanentItem>(contract);
        var command = new MakeTemporaryItemPermanentCommand(model);
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
}