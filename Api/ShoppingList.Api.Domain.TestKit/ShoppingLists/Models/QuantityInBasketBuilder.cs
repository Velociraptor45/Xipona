using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class QuantityInBasketBuilder : DomainTestBuilderBase<QuantityInBasket>
{
    public QuantityInBasketBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }
}