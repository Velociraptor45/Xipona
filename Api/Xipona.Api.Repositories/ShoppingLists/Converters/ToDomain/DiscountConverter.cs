using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Repositories.ShoppingLists.Entities;

namespace Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;

public class DiscountConverter : IToDomainConverter<Discount, Domain.ShoppingLists.Models.ItemDiscount>
{
    public Domain.ShoppingLists.Models.ItemDiscount ToDomain(Discount source)
    {
        return new(
            new ItemId(source.ItemId),
            source.ItemTypeId is null ? null : new ItemTypeId(source.ItemTypeId.Value),
            new Price(source.DiscountPrice));
    }
}