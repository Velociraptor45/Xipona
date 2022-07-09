﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemQuantityInPacketBuilder : DomainTestBuilderBase<ItemQuantityInPacket>
{
    public ItemQuantityInPacketBuilder()
    {
        Customize(new QuantityCustomization());
    }
}