using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
public class ShoppingListDiscountEntityBuilder : TestBuilderBase<ShoppingListDiscount>
{
    public ShoppingListDiscountEntityBuilder()
    {
        WithoutShoppingList();
    }

    public ShoppingListDiscountEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ShoppingListDiscountEntityBuilder WithShoppingListId(Guid shoppingListId)
    {
        FillPropertyWith(p => p.ShoppingListId, shoppingListId);
        return this;
    }

    public ShoppingListDiscountEntityBuilder WithDiscountPrice(decimal? discountPrice)
    {
        FillPropertyWith(p => p.DiscountPrice, discountPrice);
        return this;
    }

    public ShoppingListDiscountEntityBuilder WithoutDiscountPrice()
    {
        return WithDiscountPrice(null);
    }

    public ShoppingListDiscountEntityBuilder WithDiscountPercentage(decimal? discountPercentage)
    {
        FillPropertyWith(p => p.DiscountPercentage, discountPercentage);
        return this;
    }

    public ShoppingListDiscountEntityBuilder WithoutDiscountPercentage()
    {
        return WithDiscountPercentage(null);
    }

    public ShoppingListDiscountEntityBuilder WithShoppingList(ShoppingList? shoppingList)
    {
        FillPropertyWith(p => p.ShoppingList, shoppingList);
        return this;
    }

    public ShoppingListDiscountEntityBuilder WithoutShoppingList()
    {
        return WithShoppingList(null);
    }
}