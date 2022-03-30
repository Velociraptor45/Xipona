using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/manufacturers")]
public class ManufacturerController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public ManufacturerController(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ManufacturerContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [Route("search")]
    public async Task<IActionResult> GetManufacturerSearchResultsAsync([FromQuery] string searchInput)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return BadRequest("Search input mustn't be null or empty");
        }

        var query = new ManufacturerSearchQuery(searchInput);
        var readModels = (await _queryDispatcher.DispatchAsync(query, default)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters.ToContract<ManufacturerReadModel, ManufacturerContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ManufacturerContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active")]
    public async Task<IActionResult> GetAllActiveManufacturersAsync()
    {
        var query = new AllActiveManufacturersQuery();
        var readModels = (await _queryDispatcher.DispatchAsync(query, default)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters.ToContract<ManufacturerReadModel, ManufacturerContract>(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ManufacturerContract), StatusCodes.Status201Created)]
    [Route("")]
    public async Task<IActionResult> CreateManufacturerAsync([FromBody] string name)
    {
        var command = new CreateManufacturerCommand(new ManufacturerName(name));
        var model = await _commandDispatcher.DispatchAsync(command, default);

        var contract = _converters.ToContract<IManufacturer, ManufacturerContract>(model);

        //todo: change to CreatedAtAction when #47 is implemented
        return Created("", contract);
    }
}