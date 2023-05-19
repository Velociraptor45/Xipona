using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public interface IShoppingList
{
    public ShoppingListId Id { get; }
    public StoreId StoreId { get; }
    public DateTimeOffset? CompletionDate { get; }
    IReadOnlyCollection<IShoppingListSection> Sections { get; }
    public IReadOnlyCollection<IShoppingListItem> Items { get; }

    void AddItem(IShoppingListItem item, SectionId sectionId, bool throwIfAlreadyPresent = true);

    void RemoveItem(ItemId itemId);

    void RemoveItem(ItemId itemId, ItemTypeId? itemTypeId);

    void PutItemInBasket(ItemId itemId);

    void PutItemInBasket(ItemId itemId, ItemTypeId? itemTypeId);

    void RemoveFromBasket(ItemId itemId, ItemTypeId? itemTypeId);

    void ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity);

    IShoppingList Finish(DateTimeOffset completionDate);

    void AddSection(IShoppingListSection section);

    void TransferItem(SectionId sectionId, ItemId itemId, ItemTypeId? itemTypeId);

    void RemoveItemAndItsTypes(ItemId itemId);
}