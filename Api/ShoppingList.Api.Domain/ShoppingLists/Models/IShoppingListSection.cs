using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingListSection
    {
        SectionId Id { get; }
        public IReadOnlyCollection<IShoppingListItem> Items { get; }

        IShoppingListSection AddItem(IShoppingListItem item);

        IShoppingListSection ChangeItemQuantity(ItemId itemId, float quantity);

        bool ContainsItem(ItemId itemId);

        bool ContainsItem(ItemId itemId, ItemTypeId? itemTypeId);

        IShoppingListSection PutItemInBasket(ItemId itemId);

        IShoppingListSection RemoveItemFromBasket(ItemId itemId);

        IShoppingListSection RemoveItem(ItemId itemId);

        IShoppingListSection RemoveItem(ItemId itemId, ItemTypeId? itemTypeId);

        IShoppingListSection RemoveItemsInBasket();

        IShoppingListSection RemoveItemsNotInBasket();
    }
}