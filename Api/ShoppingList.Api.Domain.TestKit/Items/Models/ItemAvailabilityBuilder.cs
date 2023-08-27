using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;

public class ItemAvailabilityBuilder : DomainTestBuilderBase<ItemAvailability>
{
    private readonly List<Func<ItemAvailability, ItemAvailability>> _modifiers = new();

    public ItemAvailabilityBuilder()
    {
        Customize(new PriceCustomization());
    }

    public ItemAvailabilityBuilder WithStoreId(StoreId storeId)
    {
        _modifiers.Add(itemAvailability => itemAvailability with { StoreId = storeId });
        return this;
    }

    public ItemAvailabilityBuilder WithPrice(Price price)
    {
        _modifiers.Add(itemAvailability => itemAvailability with { Price = price });
        return this;
    }

    public ItemAvailabilityBuilder WithDefaultSectionId(SectionId defaultSectionId)
    {
        _modifiers.Add(itemAvailability => itemAvailability with { DefaultSectionId = defaultSectionId });
        return this;
    }

    public override ItemAvailability Create()
    {
        var availability = base.Create();
        foreach (var modifier in _modifiers)
        {
            availability = modifier(availability);
        }
        return availability;
    }

    public override IEnumerable<ItemAvailability> CreateMany(int count)
    {
        var availabilities = base.CreateMany(count).ToList();
        for (int i = 0; i < availabilities.Count; i++)
        {
            foreach (var modifier in _modifiers)
            {
                availabilities[i] = modifier(availabilities[i]);
            }
        }
        return availabilities;
    }
}