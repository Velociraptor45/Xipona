using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
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
        private readonly IShoppingListItemFactory shoppingListItemFactory;

        public ShoppingListUpdateService(IShoppingListRepository shoppingListRepository,
            IShoppingListItemFactory shoppingListItemFactory)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.shoppingListItemFactory = shoppingListItemFactory;
        }

        public async Task ExchangeItemAsync(ItemId oldItemId, IStoreItem newItem, CancellationToken cancellationToken)
        {
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
                    var updatedListItem = shoppingListItemFactory.Create(newItem.Id, oldListItem.IsInBasket,
                        oldListItem.Quantity);
                    list.AddItem(updatedListItem, sectionId);
                }

                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }
        }
    }
}