using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.TestKit.Common;

namespace Xipona.Api.Domain.TestKit.ShoppingLists.Models;

public class QuantityInBasketBuilder : DomainTestBuilderBase<QuantityInBasket>
{
    public QuantityInBasketBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }
}