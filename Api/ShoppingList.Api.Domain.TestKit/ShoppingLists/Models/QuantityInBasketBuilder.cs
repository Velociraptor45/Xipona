using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class QuantityInBasketBuilder : DomainTestBuilderBase<QuantityInBasket>
{
    public QuantityInBasketBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }
}