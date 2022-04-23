using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;
using ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;

namespace ShoppingList.Api.Domain.TestKit.Stores.Models;

public class StoreBuilder : DomainTestBuilderBase<Store>
{
    public StoreBuilder WithId(StoreId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public StoreBuilder WithName(StoreName name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public StoreBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith("isDeleted", isDeleted);
        return this;
    }

    public StoreBuilder WithSections(StoreSections sections)
    {
        FillConstructorWith("sections", sections);
        return this;
    }

    public StoreBuilder WithoutSections()
    {
        return WithSections(new StoreSections(Enumerable.Empty<IStoreSection>(),
            new StoreSectionFactoryMock(MockBehavior.Strict).Object));
    }
}