using Xipona.Api.Core.Services;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Models;

public interface IShoppingList
{
    public ShoppingListId Id { get; }
    public StoreId StoreId { get; }
    public DateTimeOffset? CompletionDate { get; }
    IReadOnlyCollection<IShoppingListSection> Sections { get; }
    public IReadOnlyCollection<ShoppingListItem> Items { get; }
    DateTimeOffset CreatedAt { get; }
    IReadOnlyCollection<ItemDiscount> ItemDiscounts { get; }
    IReadOnlyCollection<ListDiscount> ListDiscounts { get; }

    void AddItem(ShoppingListItem item, SectionId sectionId, bool throwIfAlreadyPresent = true);

    void RemoveItem(ItemId itemId);

    void RemoveItem(ItemId itemId, ItemTypeId? itemTypeId);

    void PutItemInBasket(ItemId itemId);

    void PutItemInBasket(ItemId itemId, ItemTypeId? itemTypeId);

    void RemoveFromBasket(ItemId itemId, ItemTypeId? itemTypeId);

    void ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity);

    IShoppingList Finish(DateTimeOffset completionDate, IDateTimeService dateTimeService);

    void AddSection(IShoppingListSection section);

    void TransferItem(SectionId sectionId, ItemId itemId, ItemTypeId? itemTypeId);

    void RemoveItemAndItsTypes(ItemId itemId);

    ItemDiscount? GetDiscountFor(ItemId itemId, ItemTypeId? itemTypeId);

    void AddDiscount(ItemDiscount discount);
    void AddDiscount(ListDiscount discount);
    void RemoveDiscount(ItemId itemId, ItemTypeId? itemTypeId);
    void RemoveDiscount(ListDiscountId discountId);
}