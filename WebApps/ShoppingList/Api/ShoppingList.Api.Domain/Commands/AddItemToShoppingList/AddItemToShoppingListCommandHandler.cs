using ShoppingList.Api.Domain.Converters;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public AddItemToShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> HandleAsync(AddItemToShoppingListCommand query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var list = await shoppingListRepository.FindByAsync(query.ShoppingListId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var storeItem = await itemRepository.FindByAsync(query.ShoppingListItemId.ToStoreItemId(),
                list.Store.Id, cancellationToken);
            var manufacturer = await manufacturerRepository
                .FindByAsync(storeItem.ManufacturerId, cancellationToken);
            var itemCategory = await itemCategoryRepository
                .FindByAsync(storeItem.ItemCategoryId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            list.AddItem(storeItem, itemCategory, manufacturer, false, query.Quantity);

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}