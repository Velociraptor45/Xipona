using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemQuantityInPacketBuilder : DomainTestBuilderBase<ItemQuantityInPacket>
{
    public ItemQuantityInPacketBuilder()
    {
        Customize(new QuantityCustomization());
    }

    public ItemQuantityInPacketBuilder WithQuantity(Quantity quantity)
    {
        FillConstructorWith(nameof(quantity), quantity);
        return this;
    }

    public ItemQuantityInPacketBuilder WithQuantityType(QuantityTypeInPacket type)
    {
        FillConstructorWith(nameof(type), type);
        return this;
    }
}