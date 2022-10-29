using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;

public class SectionBuilder : DomainTestBuilderBase<Section>
{
    public SectionBuilder WithId(SectionId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public SectionBuilder WithName(SectionName name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public SectionBuilder WithSortingIndex(int sortingIndex)
    {
        FillConstructorWith("sortingIndex", sortingIndex);
        return this;
    }

    public SectionBuilder WithIsDefaultSection(bool isDefaultSection)
    {
        FillConstructorWith("isDefaultSection", isDefaultSection);
        return this;
    }

    public SectionBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith(nameof(isDeleted), isDeleted);
        return this;
    }
}