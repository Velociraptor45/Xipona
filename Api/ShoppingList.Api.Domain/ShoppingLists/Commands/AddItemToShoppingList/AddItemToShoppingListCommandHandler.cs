using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
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
            if (list == null)
                throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

            cancellationToken.ThrowIfCancellationRequested();

            ItemId itemId = command.ShoppingListItemId.ToStoreItemId();
            var storeItem = await itemRepository.FindByAsync(itemId, cancellationToken);

            if (storeItem == null)
                throw new DomainException(new ItemNotFoundReason(itemId));

            cancellationToken.ThrowIfCancellationRequested();

            var priceAtStore = storeItem.Availabilities
                        .FirstOrDefault(av => av.Store.Id == list.StoreId)?
                        .Price;

            if (priceAtStore == null)
                throw new DomainException(new ItemAtStoreNotAvailableReason(itemId, list.StoreId));

            IShoppingListItem listItem = shoppingListItemFactory.Create(command.ShoppingListItemId, isInBasket: false,
                command.Quantity);

            list = list.AddItem(listItem, command.SectionId);

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}