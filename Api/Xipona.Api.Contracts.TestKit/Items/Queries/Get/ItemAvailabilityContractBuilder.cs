using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;
public class ItemAvailabilityContractBuilder : TestBuilderBase<ItemAvailabilityContract>
{
    public ItemAvailabilityContractBuilder WithStore(ItemStoreContract store)
    {
        FillConstructorWith(nameof(store), store);
        return this;
    }

    public ItemAvailabilityContractBuilder WithoutStore()
    {
        return WithStore(null);
    }

    public ItemAvailabilityContractBuilder WithPrice(float price)
    {
        FillConstructorWith(nameof(price), price);
        return this;
    }

    public ItemAvailabilityContractBuilder WithDefaultSection(ItemSectionContract defaultSection)
    {
        FillConstructorWith(nameof(defaultSection), defaultSection);
        return this;
    }

    public ItemAvailabilityContractBuilder WithoutDefaultSection()
    {
        return WithDefaultSection(null);
    }
}