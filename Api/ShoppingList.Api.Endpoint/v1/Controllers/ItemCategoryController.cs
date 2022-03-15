using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common;
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

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/item-category")]
public class ItemCategoryController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> _itemCategoryContractConverter;

    public ItemCategoryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> itemCategoryContractConverter)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _itemCategoryContractConverter = itemCategoryContractConverter;
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
    [ProducesResponseType(200)]
    [Route("create/{name}")]
    public async Task<IActionResult> CreateItemCategory([FromRoute(Name = "name")] string name)
    {
        var command = new CreateItemCategoryCommand(name);
        await _commandDispatcher.DispatchAsync(command, default);

        return Ok();
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