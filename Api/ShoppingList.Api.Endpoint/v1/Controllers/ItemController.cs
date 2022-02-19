using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemForShoppingLists;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/item")]
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
    [ProducesResponseType(200)]
    [Route("create")]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemContract createItemContract)
    {
        var model = _converters.ToDomain<CreateItemContract, ItemCreation>(createItemContract);
        var command = new CreateItemCommand(model);

        await _commandDispatcher.DispatchAsync(command, default);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Route("create-with-types")]
    public async Task<IActionResult> CreateItemWithTypes([FromBody] CreateItemWithTypesContract contract)
    {
        try
        {
            var model = _converters.ToDomain<CreateItemWithTypesContract, IStoreItem>(contract);
            var command = new CreateItemWithTypesCommand(model);
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
    [Route("modify-with-types")]
    public async Task<IActionResult> ModifyItemWithTypesAsync([FromBody] ModifyItemWithTypesContract contract)
    {
        var model = _converters.ToDomain<ModifyItemWithTypesContract, ItemWithTypesModification>(contract);
        var command = new ModifyItemWithTypesCommand(model);

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
    [Route("modify")]
    public async Task<IActionResult> ModifyItemAsync([FromBody] ModifyItemContract modifyItemContract)
    {
        var model = _converters.ToDomain<ModifyItemContract, ItemModify>(modifyItemContract);
        var command = new ModifyItemCommand(model);

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
    [Route("update")]
    public async Task<IActionResult> UpdateItemAsync([FromBody] UpdateItemContract updateItemContract)
    {
        var model = _converters.ToDomain<UpdateItemContract, ItemUpdate>(updateItemContract);
        var command = new UpdateItemCommand(model);

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
    [Route("update-with-types")]
    public async Task<IActionResult> UpdateItemWithTypesAsync([FromBody] UpdateItemWithTypesContract contract)
    {
        var command = _converters.ToDomain<UpdateItemWithTypesContract, UpdateItemWithTypesCommand>(contract);

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

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("search/{searchInput}/{storeId}")]
    public async Task<IActionResult> SearchItemForShoppingListAsync([FromRoute(Name = "searchInput")] string searchInput,
        [FromRoute(Name = "storeId")] int storeId)
    {
        var query = new SearchItemForShoppingListQuery(searchInput, new StoreId(storeId));

        IEnumerable<ItemForShoppingListSearchReadModel> readModels;
        try
        {
            readModels = await _queryDispatcher.DispatchAsync(query, default);
        }
        catch (DomainException e)
        {
            return BadRequest(e.Reason);
        }

        var contracts =
            _converters.ToContract<ItemForShoppingListSearchReadModel, ItemForShoppingListSearchContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route("filter")]
    public async Task<IActionResult> GetItemFilterResults([FromQuery] IEnumerable<int> storeIds,
        [FromQuery] IEnumerable<int> itemCategoryIds,
        [FromQuery] IEnumerable<int> manufacturerIds)
    {
        var query = new ItemFilterResultsQuery(
            storeIds.Select(id => new StoreId(id)),
            itemCategoryIds.Select(id => new ItemCategoryId(id)),
            manufacturerIds.Select(id => new ManufacturerId(id)));

        var readModels = await _queryDispatcher.DispatchAsync(query, default);
        var contracts = _converters.ToContract<ItemFilterResultReadModel, ItemFilterResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Route("delete/{itemId}")]
    public async Task<IActionResult> DeleteItem([FromRoute(Name = "itemId")] int itemId)
    {
        var command = new DeleteItemCommand(new ItemId(itemId));
        await _commandDispatcher.DispatchAsync(command, default);

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("{itemId}")]
    public async Task<IActionResult> Get([FromRoute(Name = "itemId")] int itemId)
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