using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;
public class ItemStoreContractBuilder : TestBuilderBase<ItemStoreContract>
{
    public ItemStoreContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemStoreContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemStoreContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public ItemStoreContractBuilder WithSections(IEnumerable<ItemSectionContract> sections)
    {
        FillConstructorWith(nameof(sections), sections);
        return this;
    }

    public ItemStoreContractBuilder WithEmptySections()
    {
        return WithSections(Enumerable.Empty<ItemSectionContract>());
    }
}