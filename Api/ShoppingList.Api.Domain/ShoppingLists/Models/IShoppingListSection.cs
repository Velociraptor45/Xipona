using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public interface IShoppingListSection
{
    SectionId Id { get; }
    public IReadOnlyCollection<IShoppingListItem> Items { get; }

    IShoppingListSection AddItem(IShoppingListItem item);

    IShoppingListSection ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, float quantity);

    bool ContainsItem(ItemId itemId);

    bool ContainsItem(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection PutItemInBasket(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection RemoveItemFromBasket(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection RemoveItem(ItemId itemId);

    IShoppingListSection RemoveItem(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection RemoveItemsInBasket();

    IShoppingListSection RemoveItemsNotInBasket();
}