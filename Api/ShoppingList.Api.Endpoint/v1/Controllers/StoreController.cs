using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/store")]
public class StoreController : ControllerBase
{
    private readonly IQueryDispatcher queryDispatcher;
    private readonly ICommandDispatcher commandDispatcher;
    private readonly IToContractConverter<StoreReadModel, ActiveStoreContract> activeStoreToContractConverter;
    private readonly IToDomainConverter<UpdateStoreContract, StoreUpdate> storeUpdateConverter;
    private readonly IToDomainConverter<CreateStoreContract, StoreCreationInfo> storeCreationInfoConverter;

    public StoreController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IToContractConverter<StoreReadModel, ActiveStoreContract> activeStoreToContractConverter,
        IToDomainConverter<UpdateStoreContract, StoreUpdate> storeUpdateConverter,
        IToDomainConverter<CreateStoreContract, StoreCreationInfo> storeCreationInfoConverter)
    {
        this.queryDispatcher = queryDispatcher;
        this.commandDispatcher = commandDispatcher;
        this.activeStoreToContractConverter = activeStoreToContractConverter;
        this.storeUpdateConverter = storeUpdateConverter;
        this.storeCreationInfoConverter = storeCreationInfoConverter;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route("active")]
    public async Task<IActionResult> GetAllActiveStores()
    {
        var query = new AllActiveStoresQuery();
        var storeReadModels = await queryDispatcher.DispatchAsync(query, default);
        var storeContracts = activeStoreToContractConverter.ToContract(storeReadModels);

        return Ok(storeContracts);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Route("create")]
    public async Task<IActionResult> CreateStore([FromBody] CreateStoreContract createStoreContract)
    {
        var model = storeCreationInfoConverter.ToDomain(createStoreContract);
        var command = new CreateStoreCommand(model);

        await commandDispatcher.DispatchAsync(command, default);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("update")]
    public async Task<IActionResult> UpdateStore([FromBody] UpdateStoreContract updateStoreContract)
    {
        var model = storeUpdateConverter.ToDomain(updateStoreContract);
        var command = new UpdateStoreCommand(model);

        try
        {
            await commandDispatcher.DispatchAsync(command, default);
        }
        catch (DomainException e)
        {
            return BadRequest(e.Reason);
        }

        return Ok();
    }
}