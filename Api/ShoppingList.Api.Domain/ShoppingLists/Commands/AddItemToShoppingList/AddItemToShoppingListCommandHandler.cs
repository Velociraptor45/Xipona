using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IItemRepository itemRepository;
        private readonly IShoppingListItemFactory shoppingListItemFactory;

        public AddItemToShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository, IShoppingListItemFactory shoppingListItemFactory)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.itemRepository = itemRepository;
            this.shoppingListItemFactory = shoppingListItemFactory;
        }

        public async Task<bool> HandleAsync(AddItemToShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var storeItem = await itemRepository.FindByAsync(command.ShoppingListItemId.ToStoreItemId(),
                list.Store.Id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var priceAtStore = storeItem.Availabilities
                        .First(av => av.StoreId == list.Store.Id)
                        .Price;

            IShoppingListItem listItem = shoppingListItemFactory.Create(storeItem, priceAtStore,
                isInBasket: false, command.Quantity);

            list.AddItem(listItem);

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}