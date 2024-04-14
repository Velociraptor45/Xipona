﻿using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands;

public class UpdateItemPriceCommand : ICommand<bool>
{
    public UpdateItemPriceCommand(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, Price price)
    {
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        StoreId = storeId;
        Price = price;
    }

    public StoreId StoreId { get; }
    public ItemId ItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
    public Price Price { get; }
}