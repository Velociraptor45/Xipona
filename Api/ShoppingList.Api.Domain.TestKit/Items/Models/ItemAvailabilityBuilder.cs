using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemAvailabilityBuilder : DomainTestBuilderBase<ItemAvailability>
{
    public ItemAvailabilityBuilder()
    {
        Customize(new PriceCustomization());
    }

    public ItemAvailabilityBuilder WithStoreId(StoreId storeId)
    {
        FillConstructorWith("storeId", storeId);
        return this;
    }

    public ItemAvailabilityBuilder WithPrice(float price)
    {
        FillConstructorWith("price", price);
        return this;
    }

    public ItemAvailabilityBuilder WithDefaultSectionId(SectionId defaultSectionId)
    {
        FillConstructorWith("defaultSectionId", defaultSectionId);
        return this;
    }
}