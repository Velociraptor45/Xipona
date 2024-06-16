using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;
public class ItemSectionContractBuilder : TestBuilderBase<ItemSectionContract>
{
    public ItemSectionContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemSectionContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemSectionContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public ItemSectionContractBuilder WithSortingIndex(int sortingIndex)
    {
        FillConstructorWith(nameof(sortingIndex), sortingIndex);
        return this;
    }
}