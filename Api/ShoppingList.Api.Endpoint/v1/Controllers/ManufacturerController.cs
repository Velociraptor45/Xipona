using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/manufacturer")]
public class ManufacturerController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IToContractConverter<ManufacturerReadModel, ManufacturerContract> _manufacturerContractConverter;

    public ManufacturerController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IToContractConverter<ManufacturerReadModel, ManufacturerContract> manufacturerContractConverter)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _manufacturerContractConverter = manufacturerContractConverter;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("search/{searchInput}")]
    public async Task<IActionResult> GetManufacturerSearchResults([FromRoute(Name = "searchInput")] string searchInput)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return BadRequest("Search input mustn't be null or empty");
        }

        var query = new ManufacturerSearchQuery(searchInput);
        var manufacturerReadModels = await _queryDispatcher.DispatchAsync(query, default);
        var manufacturerContracts = _manufacturerContractConverter.ToContract(manufacturerReadModels);

        return Ok(manufacturerContracts);
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route("all/active")]
    public async Task<IActionResult> GetAllActiveManufacturers()
    {
        var query = new AllActiveManufacturersQuery();
        var readModels = await _queryDispatcher.DispatchAsync(query, default);
        var contracts = _manufacturerContractConverter.ToContract(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Route("create/{name}")]
    public async Task<IActionResult> CreateManufacturer([FromRoute(Name = "name")] string name)
    {
        var command = new CreateManufacturerCommand(new ManufacturerName(name));
        await _commandDispatcher.DispatchAsync(command, default);

        return Ok();
    }
}