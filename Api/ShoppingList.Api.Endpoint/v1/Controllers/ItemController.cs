using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common;
using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
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
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
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
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IToContractConverter<StoreItemReadModel, StoreItemContract> _storeItemContractConverter;
        private readonly IToContractConverter<ItemSearchReadModel, ItemSearchContract> _itemSearchContractConverter;
        private readonly IToContractConverter<ItemFilterResultReadModel, ItemFilterResultContract> _itemFilterResultContractConverter;
        private readonly IToDomainConverter<CreateItemContract, ItemCreation> _itemCreationConverter;
        private readonly IToDomainConverter<CreateItemWithTypesContract, IStoreItem> _createItemWithTypesConverter;
        private readonly IToDomainConverter<ModifyItemContract, ItemModify> _itemModifyConverter;
        private readonly IToDomainConverter<ModifyItemWithTypesContract, ItemWithTypesModification> _modifyItemWithTypesConverter;
        private readonly IToDomainConverter<MakeTemporaryItemPermanentContract, PermanentItem> _permanentItemConverter;
        private readonly IToDomainConverter<CreateTemporaryItemContract, TemporaryItemCreation> _temporaryItemConverter;
        private readonly IToDomainConverter<UpdateItemContract, ItemUpdate> _itemUpdateConverter;

        public ItemController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
            IToContractConverter<StoreItemReadModel, StoreItemContract> storeItemContractConverter,
            IToContractConverter<ItemSearchReadModel, ItemSearchContract> itemSearchContractConverter,
            IToContractConverter<ItemFilterResultReadModel, ItemFilterResultContract> itemFilterResultContractConverter,
            IToDomainConverter<CreateItemContract, ItemCreation> itemCreationConverter,
            IToDomainConverter<CreateItemWithTypesContract, IStoreItem> createItemWithTypesConverter,
            IToDomainConverter<ModifyItemContract, ItemModify> itemModifyConverter,
            IToDomainConverter<ModifyItemWithTypesContract, ItemWithTypesModification> modifyItemWithTypesConverter,
            IToDomainConverter<MakeTemporaryItemPermanentContract, PermanentItem> permanentItemConverter,
            IToDomainConverter<CreateTemporaryItemContract, TemporaryItemCreation> temporaryItemConverter,
            IToDomainConverter<UpdateItemContract, ItemUpdate> itemUpdateConverter)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
            _storeItemContractConverter = storeItemContractConverter;
            _itemSearchContractConverter = itemSearchContractConverter;
            _itemFilterResultContractConverter = itemFilterResultContractConverter;
            _itemCreationConverter = itemCreationConverter;
            _createItemWithTypesConverter = createItemWithTypesConverter;
            _itemModifyConverter = itemModifyConverter;
            _modifyItemWithTypesConverter = modifyItemWithTypesConverter;
            _permanentItemConverter = permanentItemConverter;
            _temporaryItemConverter = temporaryItemConverter;
            _itemUpdateConverter = itemUpdateConverter;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemContract createItemContract)
        {
            var model = _itemCreationConverter.ToDomain(createItemContract);
            var command = new CreateItemCommand(model);

            await _commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create-with-type")]
        public async Task<IActionResult> CreateItemWithTypes([FromBody] CreateItemWithTypesContract contract)
        {
            var model = _createItemWithTypesConverter.ToDomain(contract);
            var command = new CreateItemWithTypesCommand(model);

            await _commandDispatcher.DispatchAsync(command, default);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("modify-with-type")]
        public async Task<IActionResult> ModifyItemWithTypes([FromBody] ModifyItemWithTypesContract contract)
        {
            var model = _modifyItemWithTypesConverter.ToDomain(contract);
            var command = new ModifyItemCommand(model);

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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("modify")]
        public async Task<IActionResult> ModifyItem([FromBody] ModifyItemContract modifyItemContract)
        {
            var model = _itemModifyConverter.ToDomain(modifyItemContract);
            var command = new ModifyItemCommand(model);

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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("update")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemContract updateItemContract)
        {
            var model = _itemUpdateConverter.ToDomain(updateItemContract);
            var command = new UpdateItemCommand(model);

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
                readModels = await _queryDispatcher.DispatchAsync(query, default);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
            }

            var contracts = _itemSearchContractConverter.ToContract(readModels);

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

            var readModels = await _queryDispatcher.DispatchAsync(query, default);
            var contracts = _itemFilterResultContractConverter.ToContract(readModels);

            return Ok(contracts);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("delete/{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute(Name = "itemId")] int itemId)
        {
            var command = new DeleteItemCommand(new Domain.StoreItems.Models.ItemId(itemId));
            await _commandDispatcher.DispatchAsync(command, default);

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
                result = await _queryDispatcher.DispatchAsync(query, default);
            }
            catch (DomainException e)
            {
                return BadRequest(e.Reason);
            }

            var contract = _storeItemContractConverter.ToContract(result);

            return Ok(contract);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [Route("create/temporary")]
        public async Task<IActionResult> CreateTemporaryItem([FromBody] CreateTemporaryItemContract contract)
        {
            var model = _temporaryItemConverter.ToDomain(contract);
            var command = new CreateTemporaryItemCommand(model);
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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("make-temporary-item-permanent")]
        public async Task<IActionResult> MakeTemporaryItemPermanent([FromBody] MakeTemporaryItemPermanentContract contract)
        {
            var model = _permanentItemConverter.ToDomain(contract);
            var command = new MakeTemporaryItemPermanentCommand(model);
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
}