using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Stores.Queries.Get;
public class StoreContractBuilder : TestBuilderBase<StoreContract>
{
    public StoreContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public StoreContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public StoreContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public StoreContractBuilder WithSections(IEnumerable<SectionContract> sections)
    {
        FillConstructorWith(nameof(sections), sections);
        return this;
    }

    public StoreContractBuilder WithEmptySections()
    {
        return WithSections(Enumerable.Empty<SectionContract>());
    }
}