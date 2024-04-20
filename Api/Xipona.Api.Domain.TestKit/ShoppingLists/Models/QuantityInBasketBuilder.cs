using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

public class QuantityInBasketBuilder : DomainTestBuilderBase<QuantityInBasket>
{
    public QuantityInBasketBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }
}