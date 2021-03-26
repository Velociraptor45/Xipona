using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
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

            IStoreItem storeItem;
            if (command.ItemId.IsActualId)
            {
                var actualId = new ItemId(command.ItemId.ActualId.Value);
                storeItem = await itemRepository.FindByAsync(actualId, cancellationToken);
            }
            else
            {
                var temporaryId = new TemporaryItemId(command.ItemId.OfflineId.Value);
                storeItem = await itemRepository.FindByAsync(temporaryId, cancellationToken);
            }

            if (storeItem == null)
                throw new DomainException(new ItemNotFoundReason(command.ItemId));

            cancellationToken.ThrowIfCancellationRequested();

            if (!storeItem.Availabilities.Any(av => av.StoreId == list.StoreId))
                throw new DomainException(new ItemAtStoreNotAvailableReason(storeItem.Id, list.StoreId));

            IShoppingListItem listItem = shoppingListItemFactory.Create(storeItem.Id, isInBasket: false,
                command.Quantity);

            list.AddItem(listItem, command.SectionId);

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}