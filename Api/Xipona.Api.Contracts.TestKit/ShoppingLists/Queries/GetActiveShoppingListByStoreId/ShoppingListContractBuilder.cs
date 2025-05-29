using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
public class ShoppingListContractBuilder : ContractTestBuilderBase<ShoppingListContract>
{
    public ShoppingListContractBuilder()
    {
        WithShoppingListDiscounts(ShoppingListDiscountContractMother.Percentage().CreateMany(2));
    }

    public ShoppingListContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ShoppingListContractBuilder WithStore(ShoppingListStoreContract store)
    {
        FillConstructorWith(nameof(store), store);
        return this;
    }

    public ShoppingListContractBuilder WithoutStore()
    {
        return WithStore(null);
    }

    public ShoppingListContractBuilder WithSections(IEnumerable<ShoppingListSectionContract> sections)
    {
        FillConstructorWith(nameof(sections), sections);
        return this;
    }

    public ShoppingListContractBuilder WithEmptySections()
    {
        return WithSections(Enumerable.Empty<ShoppingListSectionContract>());
    }

    public ShoppingListContractBuilder WithCompletionDate(DateTimeOffset? completionDate)
    {
        FillConstructorWith(nameof(completionDate), completionDate);
        return this;
    }

    public ShoppingListContractBuilder WithoutCompletionDate()
    {
        return WithCompletionDate(null);
    }

    public ShoppingListContractBuilder WithShoppingListDiscounts(IEnumerable<ShoppingListDiscountContract> shoppingListDiscounts)
    {
        FillConstructorWith(nameof(shoppingListDiscounts), shoppingListDiscounts);
        return this;
    }

    public ShoppingListContractBuilder WithEmptyShoppingListDiscounts()
    {
        return WithShoppingListDiscounts(Enumerable.Empty<ShoppingListDiscountContract>());
    }
}