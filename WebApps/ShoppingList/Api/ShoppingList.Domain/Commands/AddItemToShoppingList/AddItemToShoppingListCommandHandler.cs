using ShoppingList.Domain.Converters;
using ShoppingList.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IItemRepository itemRepository;

        public AddItemToShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(AddItemToShoppingListCommand query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var list = await shoppingListRepository.FindByAsync(query.ShoppingListId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var storeItem = await itemRepository.FindByAsync(query.ShoppingListItemId.ToStoreItemId(),
                list.Store.Id, cancellationToken);
            var manufacturer = await shoppingListRepository
                .FindManufacturerByAsync(storeItem.ManufacturerId, cancellationToken);
            var itemCategory = await shoppingListRepository
                .FindItemCategoryByAsync(storeItem.ItemCategoryId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            list.AddItem(storeItem, itemCategory, manufacturer, false, query.Quantity);

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}