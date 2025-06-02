using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

public readonly record struct ItemDiscount
{

    public ItemDiscount()
    {
        throw new NotSupportedException("Empty ctor not supported.");
    }

    public ItemDiscount(ItemId itemId, Price price) : this(itemId, null, price)
    {
    }

    public ItemDiscount(ItemId itemId, ItemTypeId? itemTypeId, Price price)
    {
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        Price = price;
    }

    public ItemId ItemId { get; init; }
    public ItemTypeId? ItemTypeId { get; init; }
    public Price Price { get; init; }

    public override string ToString()
    {
        return $"{ItemId} - {ItemTypeId} : {Price}";
    }
}