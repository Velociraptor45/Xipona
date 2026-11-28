using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.TestKit.Common;

namespace Xipona.Api.Domain.TestKit.ShoppingLists.Models;
public class ShoppingListBuilder : DomainTestBuilderBase<ShoppingList>
{
    public ShoppingListBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }

    // tcg keep
    public ShoppingListBuilder WithSection(IShoppingListSection section)
    {
        return WithSections([section]);
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

    public ShoppingListBuilder WithItemDiscounts(IEnumerable<ItemDiscount> itemDiscounts)
    {
        FillConstructorWith(nameof(itemDiscounts), itemDiscounts);
        return this;
    }

    public ShoppingListBuilder WithEmptyItemDiscounts()
    {
        return WithItemDiscounts(Enumerable.Empty<ItemDiscount>());
    }
}