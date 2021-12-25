using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdate
{
    public class ItemUpdateService : IItemUpdateService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTypeFactory _itemTypeFactory;
        private readonly IStoreItemFactory _storeItemFactory;
        private readonly IShoppingListUpdateService _shoppingListUpdateService;
        private readonly IValidator _validator;
        private readonly CancellationToken _cancellationToken;

        public ItemUpdateService(
            IItemRepository itemRepository,
            Func<CancellationToken, IValidator> validatorDelegate,
            IItemTypeFactory itemTypeFactory,
            IStoreItemFactory storeItemFactory,
            IShoppingListUpdateService shoppingListUpdateService,
            CancellationToken cancellationToken)
        {
            _itemRepository = itemRepository;
            _itemTypeFactory = itemTypeFactory;
            _storeItemFactory = storeItemFactory;
            _shoppingListUpdateService = shoppingListUpdateService;
            _validator = validatorDelegate(cancellationToken);
            _cancellationToken = cancellationToken;
        }

        public async Task UpdateItemWithTypesAsync(ItemWithTypesUpdate update)
        {
            if (update is null)
                throw new ArgumentNullException(nameof(update));

            var oldItem = await _itemRepository.FindByAsync(update.OldId, _cancellationToken);
            if (oldItem == null)
                throw new DomainException(new ItemNotFoundReason(update.OldId));
            if (!oldItem.HasItemTypes)
                throw new DomainException(new CannotUpdateItemAsItemWithTypesReason(update.OldId));

            oldItem.Delete();

            await _validator.ValidateAsync(update.ItemCategoryId);

            if (update.ManufacturerId != null)
            {
                await _validator.ValidateAsync(update.ManufacturerId);
            }

            _cancellationToken.ThrowIfCancellationRequested();

            var types = new List<IItemType>();
            foreach (var typeUpdate in update.TypeUpdates)
            {
                if (!oldItem.ItemTypes.TryGetValue(typeUpdate.OldId, out var predecessorType))
                    throw new DomainException(new ItemTypeNotFoundReason(oldItem.Id, typeUpdate.OldId));

                var type = _itemTypeFactory.CreateNew(typeUpdate.Name, typeUpdate.Availabilities, predecessorType!);

                await _validator.ValidateAsync(type.Availabilities);

                types.Add(type);
            }

            _cancellationToken.ThrowIfCancellationRequested();

            // create new Item
            var updatedItem = _storeItemFactory.CreateNew(
                update.Name,
                update.Comment,
                update.QuantityType,
                update.QuantityInPacket,
                update.QuantityTypeInPacket,
                update.ItemCategoryId,
                update.ManufacturerId,
                oldItem,
                types);

            await _itemRepository.StoreAsync(oldItem, _cancellationToken);
            updatedItem = await _itemRepository.StoreAsync(updatedItem, _cancellationToken);

            _cancellationToken.ThrowIfCancellationRequested();

            // change existing item references on shopping lists
            await _shoppingListUpdateService.ExchangeItemAsync(oldItem.Id, updatedItem, _cancellationToken);
        }
    }
}