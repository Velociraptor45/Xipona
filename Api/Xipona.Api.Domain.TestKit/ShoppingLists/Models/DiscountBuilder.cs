using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.TestKit.Common;

namespace Xipona.Api.Domain.TestKit.ShoppingLists.Models;
public class DiscountBuilder : DomainTestBuilderBase<ItemDiscount>
{
    public DiscountBuilder WithItemId(ItemId itemId)
    {
        FillConstructorWith("ItemId", itemId);
        return this;
    }

    public DiscountBuilder WithItemTypeId(ItemTypeId? itemTypeId)
    {
        FillConstructorWith("ItemTypeId", itemTypeId);
        return this;
    }

    public DiscountBuilder WithoutItemTypeId()
    {
        return WithItemTypeId(null);
    }

    public DiscountBuilder WithPrice(Price price)
    {
        FillConstructorWith("Price", price);
        return this;
    }
}