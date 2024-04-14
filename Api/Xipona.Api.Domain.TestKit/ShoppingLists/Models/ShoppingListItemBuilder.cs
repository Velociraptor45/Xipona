using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListItemBuilder : DomainRecordTestBuilderBase<ShoppingListItem>
{
    public ShoppingListItemBuilder WithId(ItemId id)
    {
        Modifiers.Add(item => item with { Id = id });
        return this;
    }

    public ShoppingListItemBuilder WithIsInBasket(bool isInBasket)
    {
        Modifiers.Add(item => item with { IsInBasket = isInBasket });
        return this;
    }

    public ShoppingListItemBuilder WithQuantity(QuantityInBasket quantity)
    {
        Modifiers.Add(item => item with { Quantity = quantity });
        return this;
    }

    public ShoppingListItemBuilder WithTypeId(ItemTypeId? typeId)
    {
        Modifiers.Add(item => item with { TypeId = typeId });
        return this;
    }

    public ShoppingListItemBuilder WithoutTypeId()
    {
        return WithTypeId(null);
    }
}