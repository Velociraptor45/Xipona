using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/store")]
public class StoreController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IToContractConverter<StoreReadModel, ActiveStoreContract> _activeStoreToContractConverter;
    private readonly IEndpointConverters _converters;

    public StoreController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IToContractConverter<StoreReadModel, ActiveStoreContract> activeStoreToContractConverter,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _activeStoreToContractConverter = activeStoreToContractConverter;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route("active")]
    public async Task<IActionResult> GetAllActiveStores()
    {
        var query = new AllActiveStoresQuery();
        var storeReadModels = await _queryDispatcher.DispatchAsync(query, default);
        var storeContracts = _activeStoreToContractConverter.ToContract(storeReadModels);

        return Ok(storeContracts);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Route("create")]
    public async Task<IActionResult> CreateStore([FromBody] CreateStoreContract createStoreContract)
    {
        var command = _converters.ToDomain<CreateStoreContract, CreateStoreCommand>(createStoreContract);
        await _commandDispatcher.DispatchAsync(command, default);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("update")]
    public async Task<IActionResult> UpdateStore([FromBody] UpdateStoreContract updateStoreContract)
    {
        var command = _converters.ToDomain<UpdateStoreContract, UpdateStoreCommand>(updateStoreContract);

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