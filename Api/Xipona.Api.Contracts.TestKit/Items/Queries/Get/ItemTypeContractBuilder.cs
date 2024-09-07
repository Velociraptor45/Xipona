using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;
public class ItemTypeContractBuilder : TestBuilderBase<ItemTypeContract>
{
    public ItemTypeContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemTypeContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemTypeContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public ItemTypeContractBuilder WithAvailabilities(IEnumerable<ItemAvailabilityContract> availabilities)
    {
        FillConstructorWith(nameof(availabilities), availabilities);
        return this;
    }

    public ItemTypeContractBuilder WithEmptyAvailabilities()
    {
        return WithAvailabilities(Enumerable.Empty<ItemAvailabilityContract>());
    }
}