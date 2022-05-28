using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/item-categories")]
public class ItemCategoryController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> _itemCategoryContractConverter;
    private readonly IEndpointConverters _converters;

    public ItemCategoryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _itemCategoryContractConverter = itemCategoryContractConverter;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("")]
    public async Task<IActionResult> SearchItemCategoriesByNameAsync([FromQuery] string searchInput)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return BadRequest("Search input mustn't be null or empty");
        }

        var query = new ItemCategorySearchQuery(searchInput);
        var itemCategoryReadModels = await _queryDispatcher.DispatchAsync(query, default);

        if (!itemCategoryReadModels.Any())
            return NoContent();

        var itemCategoryContracts = _itemCategoryContractConverter.ToContract(itemCategoryReadModels);

        return Ok(itemCategoryContracts);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active")]
    public async Task<IActionResult> GetAllActiveItemCategoriesAsync()
    {
        var query = new AllActiveItemCategoriesQuery();
        var readModels = await _queryDispatcher.DispatchAsync(query, default);

        if (!readModels.Any())
            return NoContent();

        var contracts = _itemCategoryContractConverter.ToContract(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Route("")]
    public async Task<IActionResult> CreateItemCategoryAsync([FromQuery] string name)
    {
        var command = new CreateItemCategoryCommand(new ItemCategoryName(name));
        var model = await _commandDispatcher.DispatchAsync(command, default);

        var contract = _converters.ToContract<IItemCategory, ItemCategoryContract>(model);

        //todo: change to CreatedAtAction when #47 is implemented
        return Created("", contract);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Route("{id}")]
    public async Task<IActionResult> DeleteItemCategoryAsync([FromRoute] Guid id)
    {
        try
        {
            var command = new DeleteItemCategoryCommand(new ItemCategoryId(id));
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return Ok();
    }
}