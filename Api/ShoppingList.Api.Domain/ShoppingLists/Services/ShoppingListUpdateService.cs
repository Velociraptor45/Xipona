using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services
{
    public class ShoppingListUpdateService : IShoppingListUpdateService
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IAddItemToShoppingListService addItemToShoppingListService;

        public ShoppingListUpdateService(IShoppingListRepository shoppingListRepository,
            IAddItemToShoppingListService addItemToShoppingListService)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.addItemToShoppingListService = addItemToShoppingListService;
        }

        public async Task ExchangeItemAsync(ItemId oldItemId, IStoreItem newItem, CancellationToken cancellationToken)
        {
            if (oldItemId is null)
                throw new System.ArgumentNullException(nameof(oldItemId));
            if (newItem is null)
                throw new System.ArgumentNullException(nameof(newItem));

            var shoppingListsWithOldItem = (await shoppingListRepository
                .FindActiveByAsync(oldItemId, cancellationToken))
                .ToList();

            foreach (var list in shoppingListsWithOldItem)
            {
                IShoppingListItem oldListItem = list.Items
                    .First(i => i.Id == oldItemId);
                list.RemoveItem(oldListItem.Id);
                if (newItem.IsAvailableInStore(list.StoreId))
                {
                    var sectionId = newItem.GetDefaultSectionIdForStore(list.StoreId);
                    await addItemToShoppingListService.AddItemToShoppingList(list, newItem.Id, sectionId,
                        oldListItem.Quantity, cancellationToken);

                    if (oldListItem.IsInBasket)
                        list.PutItemInBasket(newItem.Id);
                }

                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }
        }
    }
}