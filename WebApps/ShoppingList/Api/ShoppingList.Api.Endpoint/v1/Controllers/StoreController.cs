﻿using Microsoft.AspNetCore.Mvc;
using ShoppingList.Api.ApplicationServices;
using ShoppingList.Api.Contracts.Commands.CreateStore;
using ShoppingList.Api.Contracts.Commands.UpdateStore;
using ShoppingList.Api.Domain.Commands.CreateStore;
using ShoppingList.Api.Domain.Commands.UpdateStore;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Queries.AllActiveStores;
using ShoppingList.Api.Endpoint.Extensions.Store;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Api.Endpoint.v1.Controllers
{
    [ApiController]
    [Route("v1/store")]
    public class StoreController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public StoreController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("active")]
        public async Task<IActionResult> GetAllActiveStores()
        {
            var query = new AllActiveStoresQuery();
            var storeReadModels = await queryDispatcher.DispatchAsync(query, default);
            var storeContracts = storeReadModels.Select(store => store.ToContract());

            return Ok(storeContracts);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create")]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreContract createStoreContract)
        {
            var model = createStoreContract.ToDomain();
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
            var model = updateStoreContract.ToDomain();
            var command = new UpdateStoreCommand(model);

            try
            {
                await commandDispatcher.DispatchAsync(command, default);
            }
            catch (StoreNotFoundException)
            {
                return BadRequest("Store doesn't exist.");
            }

            return Ok();
        }
    }
}