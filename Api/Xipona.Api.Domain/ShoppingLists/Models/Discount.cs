using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

public readonly record struct Discount
{

    public Discount()
    {
        throw new NotSupportedException("Empty ctor not supported.");
    }

    public Discount(ItemId itemId, Price price) : this(itemId, null, price)
    {
    }

    public Discount(ItemId itemId, ItemTypeId? itemTypeId, Price price)
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