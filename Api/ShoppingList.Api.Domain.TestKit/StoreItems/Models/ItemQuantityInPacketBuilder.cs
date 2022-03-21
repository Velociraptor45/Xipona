using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public class ItemQuantityInPacketBuilder : DomainTestBuilderBase<ItemQuantityInPacket>
{
    public ItemQuantityInPacketBuilder()
    {
        Customize(new QuantityCustomization());
    }
}