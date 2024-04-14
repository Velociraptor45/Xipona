﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerById;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;

[ApiController]
[Authorize(Policy = "User")]
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
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetManufacturerByIdAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new ManufacturerByIdQuery(new ManufacturerId(id));

            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            var contract = _converters.ToContract<IManufacturer, ManufacturerContract>(result);

            return Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ManufacturerSearchResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [Route("")]
    public async Task<IActionResult> GetManufacturerSearchResultsAsync([FromQuery] string searchInput,
        [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return BadRequest("Search input mustn't be null or empty");
        }

        var query = new ManufacturerSearchQuery(searchInput, includeDeleted);
        var readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (readModels.Count == 0)
            return NoContent();

        var contracts = _converters.ToContract<ManufacturerSearchResultReadModel, ManufacturerSearchResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ManufacturerContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active")]
    public async Task<IActionResult> GetAllActiveManufacturersAsync(CancellationToken cancellationToken = default)
    {
        var query = new AllActiveManufacturersQuery();
        var readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (readModels.Count == 0)
            return NoContent();

        var contracts = _converters.ToContract<ManufacturerReadModel, ManufacturerContract>(readModels);

        return Ok(contracts);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("")]
    public async Task<IActionResult> ModifyManufacturerAsync([FromBody] ModifyManufacturerContract contract,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = _converters.ToDomain<ModifyManufacturerContract, ModifyManufacturerCommand>(contract);
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ManufacturerContract), StatusCodes.Status201Created)]
    [Route("")]
    public async Task<IActionResult> CreateManufacturerAsync([FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateManufacturerCommand(new ManufacturerName(name));
        var model = await _commandDispatcher.DispatchAsync(command, cancellationToken);

        var contract = _converters.ToContract<IManufacturer, ManufacturerContract>(model);

        return CreatedAtAction(nameof(GetManufacturerByIdAsync), new { id = contract.Id }, contract);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id}")]
    public async Task<IActionResult> DeleteManufacturerAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteManufacturerCommand(new ManufacturerId(id));
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }
}