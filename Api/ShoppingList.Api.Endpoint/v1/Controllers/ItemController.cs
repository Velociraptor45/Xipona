using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers
{
    [ApiController]
    [Route("v1/item")]
    public class ItemController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IToContractConverter<StoreItemReadModel, StoreItemContract> storeItemContractConverter;
        private readonly IToContractConverter<ItemSearchReadModel, ItemSearchContract> itemSearchContractConverter;
        private readonly IToContractConverter<ItemFilterResultReadModel, ItemFilterResultContract> itemFilterResultContractConverter;
        private readonly IToDomainConverter<CreateItemContract, ItemCreation> itemCreationConverter;
        private readonly IToDomainConverter<ModifyItemContract, ItemModify> itemModifyConverter;
        private readonly IToDomainConverter<MakeTemporaryItemPermanentContract, PermanentItem> permanentItemConverter;
        private readonly IToDomainConverter<CreateTemporaryItemContract, TemporaryItemCreation> temporaryItemConverter;
        private readonly IToDomainConverter<UpdateItemContract, ItemUpdate> itemUpdateConverter;

        public ItemController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
            IToContractConverter<StoreItemReadModel, StoreItemContract> storeItemContractConverter,
            IToContractConverter<ItemSearchReadModel, ItemSearchContract> itemSearchContractConverter,
            IToContractConverter<ItemFilterResultReadModel, ItemFilterResultContract> itemFilterResultContractConverter,
            IToDomainConverter<CreateItemContract, ItemCreation> itemCreationConverter,
            IToDomainConverter<ModifyItemContract, ItemModify> itemModifyConverter,
            IToDomainConverter<MakeTemporaryItemPermanentContract, PermanentItem> permanentItemConverter,
            IToDomainConverter<CreateTemporaryItemContract, TemporaryItemCreation> temporaryItemConverter,
            IToDomainConverter<UpdateItemContract, ItemUpdate> itemUpdateConverter)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.storeItemContractConverter = storeItemContractConverter;
            this.itemSearchContractConverter = itemSearchContractConverter;
            this.itemFilterResultContractConverter = itemFilterResultContractConverter;
            this.itemCreationConverter = itemCreationConverter;
            this.itemModifyConverter = itemModifyConverter;
            this.permanentItemConverter = permanentItemConverter;
            this.temporaryItemConverter = temporaryItemConverter;
            this.itemUpdateConverter = itemUpdateConverter;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemContract createItemContract)
        {
            var model = itemCreationConverter.ToDomain(createItemContract);
            var command = new CreateItemCommand(model);

            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("modify")]
        public async Task<IActionResult> ModifyItem([FromBody] ModifyItemContract modifyItemContract)
        {
            var model = itemModifyConverter.ToDomain(modifyItemContract);
            var command = new ModifyItemCommand(model);

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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("update")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemContract updateItemContract)
        {
            var model = itemUpdateConverter.ToDomain(updateItemContract);
            var command = new UpdateItemCommand(model);

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

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("search/{searchInput}/{storeId}")]
        public async Task<IActionResult> GetItemSearchResults([FromRoute(Name = "searchInput")] string searchInput,
            [FromRoute(Name = "storeId")] int storeId)
        {
            var query = new ItemSearchQuery(searchInput, new StoreId(storeId));

            IEnumerable<ItemSearchReadModel> readModels;
            try
            {
                readModels = await queryDispatcher.DispatchAsync(query, default);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
            }

            var contracts = itemSearchContractConverter.ToContract(readModels);

            return Ok(contracts);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("filter")]
        public async Task<IActionResult> GetItemFilterResults([FromQuery] IEnumerable<int> storeIds,
            [FromQuery] IEnumerable<int> itemCategoryIds,
            [FromQuery] IEnumerable<int> manufacturerIds)
        {
            var query = new ItemFilterResultsQuery(
                storeIds.Select(id => new StoreId(id)),
                itemCategoryIds.Select(id => new ItemCategoryId(id)),
                manufacturerIds.Select(id => new ManufacturerId(id)));

            var readModels = await queryDispatcher.DispatchAsync(query, default);
            var contracts = itemFilterResultContractConverter.ToContract(readModels);

            return Ok(contracts);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("delete/{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute(Name = "itemId")] int itemId)
        {
            var command = new DeleteItemCommand(new Domain.StoreItems.Models.ItemId(itemId));
            await commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("{itemId}")]
        public async Task<IActionResult> Get([FromRoute(Name = "itemId")] int itemId)
        {
            var query = new ItemByIdQuery(new Domain.StoreItems.Models.ItemId(itemId));
            StoreItemReadModel result;
            try
            {
                result = await queryDispatcher.DispatchAsync(query, default);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
            }

            var contract = storeItemContractConverter.ToContract(result);

            return Ok(contract);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create/temporary")]
        public async Task<IActionResult> CreateTemporaryItem([FromBody] CreateTemporaryItemContract contract)
        {
            var model = temporaryItemConverter.ToDomain(contract);
            var command = new CreateTemporaryItemCommand(model);
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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("make-temporary-item-permanent")]
        public async Task<IActionResult> MakeTemporaryItemPermanent([FromBody] MakeTemporaryItemPermanentContract contract)
        {
            var model = permanentItemConverter.ToDomain(contract);
            var command = new MakeTemporaryItemPermanentCommand(model);
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
}