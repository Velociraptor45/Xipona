using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/stores")]
public class StoreController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public StoreController(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ActiveStoreContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active")]
    public async Task<IActionResult> GetAllActiveStoresAsync()
    {
        var query = new AllActiveStoresQuery();
        var readModels = (await _queryDispatcher.DispatchAsync(query, default)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters.ToContract<StoreReadModel, ActiveStoreContract>(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StoreContract), StatusCodes.Status201Created)]
    [Route("")]
    public async Task<IActionResult> CreateStoreAsync([FromBody] CreateStoreContract createStoreContract)
    {
        var command = _converters.ToDomain<CreateStoreContract, CreateStoreCommand>(createStoreContract);
        var model = await _commandDispatcher.DispatchAsync(command, default);

        var contract = _converters.ToContract<IStore, StoreContract>(model);

        //todo: change to CreatedAtAction
        return Created("", contract);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("")]
    public async Task<IActionResult> UpdateStoreAsync([FromBody] UpdateStoreContract updateStoreContract)
    {
        var command = _converters.ToDomain<UpdateStoreContract, UpdateStoreCommand>(updateStoreContract);

        try
        {
            await _commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return Ok();
    }
}