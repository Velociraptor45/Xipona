namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
public static class ShoppingListDiscountEntityMother
{
    public static ShoppingListDiscountEntityBuilder Price()
    {
        return new ShoppingListDiscountEntityBuilder()
            .WithoutDiscountPercentage();
    }

    public static ShoppingListDiscountEntityBuilder Percentage()
    {
        return new ShoppingListDiscountEntityBuilder()
            .WithoutDiscountPrice();
    }
}
