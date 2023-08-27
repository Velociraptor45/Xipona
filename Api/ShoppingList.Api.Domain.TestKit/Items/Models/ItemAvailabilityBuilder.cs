using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemAvailabilityBuilder : DomainRecordTestBuilderBase<ItemAvailability>
{
    public ItemAvailabilityBuilder()
    {
        Customize(new PriceCustomization());
    }

    public ItemAvailabilityBuilder WithStoreId(StoreId storeId)
    {
        Modifiers.Add(itemAvailability => itemAvailability with { StoreId = storeId });
        return this;
    }

    public ItemAvailabilityBuilder WithPrice(Price price)
    {
        Modifiers.Add(itemAvailability => itemAvailability with { Price = price });
        return this;
    }

    public ItemAvailabilityBuilder WithDefaultSectionId(SectionId defaultSectionId)
    {
        Modifiers.Add(itemAvailability => itemAvailability with { DefaultSectionId = defaultSectionId });
        return this;
    }
}