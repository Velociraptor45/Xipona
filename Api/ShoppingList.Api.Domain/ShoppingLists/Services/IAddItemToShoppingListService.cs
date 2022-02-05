using System.Threading;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;

public interface IAddItemToShoppingListService
{
    Task AddItemToShoppingList(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId, float quantity,
        CancellationToken cancellationToken);

    Task AddItemToShoppingList(IShoppingList shoppingList, TemporaryItemId temporaryItemId, SectionId? sectionId,
        float quantity, CancellationToken cancellationToken);

    Task AddItemWithTypeToShoppingList(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, float quantity, CancellationToken cancellationToken);

    Task AddItemWithTypeToShoppingList(IShoppingList shoppingList, IStoreItem item, ItemTypeId itemTypeId,
        SectionId? sectionId, float quantity, CancellationToken cancellationToken);
}