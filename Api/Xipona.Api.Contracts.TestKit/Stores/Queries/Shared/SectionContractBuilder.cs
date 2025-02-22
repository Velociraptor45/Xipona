using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Stores.Queries.Shared;
public class SectionContractBuilder : TestBuilderBase<SectionContract>
{
    public SectionContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public SectionContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public SectionContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public SectionContractBuilder WithSortingIndex(int sortingIndex)
    {
        FillConstructorWith(nameof(sortingIndex), sortingIndex);
        return this;
    }

    public SectionContractBuilder WithIsDefaultSection(bool isDefaultSection)
    {
        FillConstructorWith(nameof(isDefaultSection), isDefaultSection);
        return this;
    }
}