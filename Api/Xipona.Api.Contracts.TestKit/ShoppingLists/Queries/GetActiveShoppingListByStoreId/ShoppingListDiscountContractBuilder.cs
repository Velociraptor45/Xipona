using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
public class ShoppingListDiscountContractBuilder : ContractTestBuilderBase<ShoppingListDiscountContract>
{
    public ShoppingListDiscountContractBuilder WithDiscountPrice(decimal? discountPrice)
    {
        FillConstructorWith(nameof(discountPrice), discountPrice);
        return this;
    }

    public ShoppingListDiscountContractBuilder WithoutDiscountPrice()
    {
        return WithDiscountPrice(null);
    }

    public ShoppingListDiscountContractBuilder WithDiscountPercentage(decimal? discountPercentage)
    {
        FillConstructorWith(nameof(discountPercentage), discountPercentage);
        return this;
    }

    public ShoppingListDiscountContractBuilder WithoutDiscountPercentage()
    {
        return WithDiscountPercentage(null);
    }
}