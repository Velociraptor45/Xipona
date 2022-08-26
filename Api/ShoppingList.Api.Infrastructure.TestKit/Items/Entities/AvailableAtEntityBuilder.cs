﻿using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;

public class AvailableAtEntityBuilder : TestBuilder<AvailableAt>
{
    public AvailableAtEntityBuilder()
    {
        WithoutItem();
    }

    public AvailableAtEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(av => av.StoreId, storeId);
        return this;
    }

    public AvailableAtEntityBuilder WithoutItem()
    {
        return WithItem(null);
    }

    public AvailableAtEntityBuilder WithItem(Item? item)
    {
        FillPropertyWith(av => av.Item, item);
        return this;
    }

    public AvailableAtEntityBuilder WithDefaultSectionId(Guid defaultSectionId)
    {
        FillPropertyWith(av => av.DefaultSectionId, defaultSectionId);
        return this;
    }
}