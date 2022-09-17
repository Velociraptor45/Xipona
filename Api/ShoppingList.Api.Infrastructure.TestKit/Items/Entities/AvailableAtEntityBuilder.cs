using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;

public class AvailableAtEntityBuilder : TestBuilderBase<AvailableAt>
{
    public AvailableAtEntityBuilder()
    {
        WithoutItem();
    }
    public AvailableAtEntityBuilder WithItemId(Guid itemId)
    {
        FillPropertyWith(p => p.ItemId, itemId);
        return this;
    }

    public AvailableAtEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(p => p.StoreId, storeId);
        return this;
    }

    public AvailableAtEntityBuilder WithPrice(float price)
    {
        FillPropertyWith(p => p.Price, price);
        return this;
    }

    public AvailableAtEntityBuilder WithDefaultSectionId(Guid defaultSectionId)
    {
        FillPropertyWith(p => p.DefaultSectionId, defaultSectionId);
        return this;
    }

    public AvailableAtEntityBuilder WithItem(Item? item)
    {
        FillPropertyWith(p => p.Item, item);
        return this;
    }

    public AvailableAtEntityBuilder WithoutItem()
    {
        return WithItem(null);
    }
}