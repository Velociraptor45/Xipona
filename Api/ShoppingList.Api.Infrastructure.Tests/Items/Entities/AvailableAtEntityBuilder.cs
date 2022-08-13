using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Tests.Items.Entities;

public class AvailableAtEntityBuilder : TestBuilder<AvailableAt>
{
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
}