using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Repositories.ShoppingLists.Entities;

namespace Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;

public class ListDiscountConverter : IToDomainConverter<ShoppingListDiscount, ListDiscount>
{
    public ListDiscount ToDomain(ShoppingListDiscount source)
    {
        if (source.DiscountPercentage is not null)
            return new(new ListDiscountId(source.Id), new Percentage(source.DiscountPercentage.Value));
        if (source.DiscountPrice is not null)
            return new(new ListDiscountId(source.Id), source.DiscountPrice.Value);

        throw new InvalidOperationException($"Discount {source.Id} does not contain a price or percentage");
    }
}