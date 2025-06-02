namespace ProjectHermes.Xipona.Api.Contracts.TestKit.ShoppingLists.Queries.GetActiveShoppingListByStoreId;

public static class ShoppingListDiscountContractMother
{
    public static ShoppingListDiscountContractBuilder Price()
    {
        return new ShoppingListDiscountContractBuilder().WithoutDiscountPercentage();
    }

    public static ShoppingListDiscountContractBuilder Percentage()
    {
        return new ShoppingListDiscountContractBuilder().WithoutDiscountPrice();
    }
}
