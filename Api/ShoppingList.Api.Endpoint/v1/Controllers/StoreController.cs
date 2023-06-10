using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.DeleteStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.ModifyStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.StoreById;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;
using System.Threading;

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
    [ProducesResponseType(typeof(StoreContract), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetStoreByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetStoreByIdQuery(new StoreId(id));

        IStore model;
        try
        {
            model = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        var contract = _converters.ToContract<IStore, StoreContract>(model);

        return Ok(contract);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StoreForShoppingContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active-for-shopping")]
    public async Task<IActionResult> GetActiveStoresForShoppingAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetActiveStoresForShoppingQuery();
        var models = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!models.Any())
            return NoContent();

        var contract = _converters.ToContract<IStore, StoreForShoppingContract>(models);
        return Ok(contract);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StoreForItemContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active-for-item")]
    public async Task<IActionResult> GetActiveStoresForItemAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetActiveStoresForItemQuery();
        var models = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!models.Any())
            return NoContent();

        var contract = _converters.ToContract<IStore, StoreForItemContract>(models);
        return Ok(contract);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StoreSearchResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active-overview")]
    public async Task<IActionResult> GetActiveStoresOverviewAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetActiveStoresOverviewQuery();
        var models = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!models.Any())
            return NoContent();

        var contract = _converters.ToContract<IStore, StoreSearchResultContract>(models);
        return Ok(contract);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StoreContract), StatusCodes.Status201Created)]
    [Route("")]
    public async Task<IActionResult> CreateStoreAsync([FromBody] CreateStoreContract createStoreContract,
        CancellationToken cancellationToken = default)
    {
        var command = _converters.ToDomain<CreateStoreContract, CreateStoreCommand>(createStoreContract);
        var model = await _commandDispatcher.DispatchAsync(command, cancellationToken);

        var contract = _converters.ToContract<IStore, StoreContract>(model);

        return CreatedAtAction(nameof(GetStoreByIdAsync), contract);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("")]
    public async Task<IActionResult> ModifyStoreAsync([FromBody] ModifyStoreContract modifyStoreContract,
        CancellationToken cancellationToken = default)
    {
        var command = _converters.ToDomain<ModifyStoreContract, ModifyStoreCommand>(modifyStoreContract);

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteStoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteStoreCommand(new StoreId(id));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
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