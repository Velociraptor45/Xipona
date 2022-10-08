using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemQuantityInPacketBuilder : DomainTestBuilderBase<ItemQuantityInPacket>
{
    public ItemQuantityInPacketBuilder()
    {
        Customize(new QuantityCustomization());
    }
}