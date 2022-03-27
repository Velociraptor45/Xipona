using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Commands;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/item-category")]
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
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("search/{searchInput}")]
    public async Task<IActionResult> GetItemCategorySearchResults([FromRoute(Name = "searchInput")] string searchInput)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return BadRequest("Search input mustn't be null or empty");
        }

        var query = new ItemCategorySearchQuery(searchInput);
        var itemCategoryReadModels = await _queryDispatcher.DispatchAsync(query, default);
        var itemCategoryContracts = _itemCategoryContractConverter.ToContract(itemCategoryReadModels);

        return Ok(itemCategoryContracts);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route("all/active")]
    public async Task<IActionResult> GetAllActiveItemCategories()
    {
        var query = new AllActiveItemCategoriesQuery();
        var readModels = await _queryDispatcher.DispatchAsync(query, default);
        var contracts = _itemCategoryContractConverter.ToContract(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Route("create/{name}")]
    public async Task<IActionResult> CreateItemCategory([FromRoute(Name = "name")] string name)
    {
        var command = new CreateItemCategoryCommand(new ItemCategoryName(name));
        var model = await _commandDispatcher.DispatchAsync(command, default);

        var contract = _converters.ToContract<IItemCategory, ItemCategoryContract>(model);

        //todo: change to CreatedAtAction when #47 is implemented
        return Created("", contract);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("delete")]
    public async Task<IActionResult> DeleteItemCategory([FromBody] DeleteItemCategoryContract contract)
    {
        var command = new DeleteItemCategoryCommand(new ItemCategoryId(contract.ItemCategoryId));
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