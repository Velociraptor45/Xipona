using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Tests.Items.Entities;

public class ItemTypeAvailableAtEntityBuilder : TestBuilder<ItemTypeAvailableAt>
{
    public ItemTypeAvailableAtEntityBuilder()
    {
    }

    public ItemTypeAvailableAtEntityBuilder(ItemTypeAvailableAt availability)
    {
        WithItemTypeId(availability.ItemTypeId);
        WithStoreId(availability.StoreId);
        WithPrice(availability.Price);
        WithDefaultSectionId(availability.DefaultSectionId);
        WithItemType(availability.ItemType);
    }

    public ItemTypeAvailableAtEntityBuilder WithItemTypeId(Guid id)
    {
        FillPropertyWith(av => av.ItemTypeId, id);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithPrice(float price)
    {
        FillPropertyWith(av => av.Price, price);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(av => av.StoreId, storeId);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithDefaultSectionId(Guid sectionId)
    {
        FillPropertyWith(av => av.DefaultSectionId, sectionId);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithoutItemType()
    {
        return WithItemType(null);
    }

    public ItemTypeAvailableAtEntityBuilder WithItemType(ItemType? itemType)
    {
        FillPropertyWith(av => av.ItemType, itemType);
        return this;
    }
}