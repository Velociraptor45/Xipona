using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemQuantityInPacketBuilder : DomainRecordTestBuilderBase<ItemQuantityInPacket>
{
    public ItemQuantityInPacketBuilder WithQuantity(Quantity quantity)
    {
        Modifiers.Add(itemQuantityInPacket => itemQuantityInPacket with { Quantity = quantity });
        return this;
    }

    public ItemQuantityInPacketBuilder WithQuantityType(QuantityTypeInPacket type)
    {
        Modifiers.Add(itemQuantityInPacket => itemQuantityInPacket with { Type = type });
        return this;
    }
}