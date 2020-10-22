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

            var storeItem = await itemRepository.FindBy(query.ShoppingListItemId.ToStoreItemId(),
                list.Store.Id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            list.AddItem(storeItem, false, query.Quantity);

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}