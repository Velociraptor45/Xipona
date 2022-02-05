using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;
using DomainModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListBuilder : DomainTestBuilderBase<DomainModels.ShoppingList>
{
    public ShoppingListBuilder WithId(DomainModels.ShoppingListId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public ShoppingListBuilder WithStoreId(StoreId storeId)
    {
        FillConstructorWith("storeId", storeId);
        return this;
    }

    public ShoppingListBuilder WithCompletionDate(DateTime? completionDate)
    {
        FillConstructorWith("completionDate", completionDate);
        return this;
    }

    public ShoppingListBuilder WithoutCompletionDate()
    {
        return WithCompletionDate(null);
    }

    public ShoppingListBuilder WithSections(IEnumerable<DomainModels.IShoppingListSection> sections)
    {
        FillConstructorWith("sections", sections);
        return this;
    }

    public ShoppingListBuilder WithSection(DomainModels.IShoppingListSection section)
    {
        return WithSections(section.ToMonoList());
    }

    public ShoppingListBuilder WithoutSections()
    {
        return WithSections(Enumerable.Empty<DomainModels.IShoppingListSection>());
    }
}