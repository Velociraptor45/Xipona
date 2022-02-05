using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.Stores.Models;

public class StoreBuilder : DomainTestBuilderBase<Store>
{
    public StoreBuilder WithId(StoreId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public StoreBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith("isDeleted", isDeleted);
        return this;
    }

    public StoreBuilder WithSections(IEnumerable<IStoreSection> sections)
    {
        FillConstructorWith("sections", sections);
        return this;
    }

    public StoreBuilder WithSection(IStoreSection section)
    {
        return WithSections(section.ToMonoList());
    }

    public StoreBuilder WithoutSections()
    {
        return WithSections(Enumerable.Empty<IStoreSection>());
    }
}