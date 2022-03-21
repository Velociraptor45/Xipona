using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public class StoreItemAvailabilityBuilder : DomainTestBuilderBase<StoreItemAvailability>
{
    public StoreItemAvailabilityBuilder()
    {
        Customize(new PriceCustomization());
    }

    public StoreItemAvailabilityBuilder WithStoreId(StoreId storeId)
    {
        FillConstructorWith("storeId", storeId);
        return this;
    }

    public StoreItemAvailabilityBuilder WithPrice(float price)
    {
        FillConstructorWith("price", price);
        return this;
    }

    public StoreItemAvailabilityBuilder WithDefaultSectionId(SectionId defaultSectionId)
    {
        FillConstructorWith("defaultSectionId", defaultSectionId);
        return this;
    }
}