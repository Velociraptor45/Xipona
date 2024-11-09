using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListBuilder : DomainTestBuilderBase<ShoppingList>
{
    public ShoppingListBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }

    public ShoppingListBuilder WithId(ShoppingListId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ShoppingListBuilder WithStoreId(StoreId storeId)
    {
        FillConstructorWith(nameof(storeId), storeId);
        return this;
    }

    public ShoppingListBuilder WithCompletionDate(DateTimeOffset? completionDate)
    {
        FillConstructorWith(nameof(completionDate), completionDate);
        return this;
    }

    public ShoppingListBuilder WithoutCompletionDate()
    {
        return WithCompletionDate(null);
    }

    // tcg keep
    public ShoppingListBuilder WithSection(IShoppingListSection section)
    {
        return WithSections([section]);
    }

    public ShoppingListBuilder WithSections(IEnumerable<IShoppingListSection> sections)
    {
        FillConstructorWith(nameof(sections), sections);
        return this;
    }

    public ShoppingListBuilder WithEmptySections()
    {
        return WithSections(Enumerable.Empty<IShoppingListSection>());
    }

    public ShoppingListBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillConstructorWith(nameof(createdAt), createdAt);
        return this;
    }

    public ShoppingListBuilder WithDiscounts(IEnumerable<Discount> discounts)
    {
        FillConstructorWith(nameof(discounts), discounts);
        return this;
    }

    public ShoppingListBuilder WithEmptyDiscounts()
    {
        return WithDiscounts(Enumerable.Empty<Discount>());
    }
}