using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;

public class DiscountConverter : IToDomainConverter<Discount, Domain.ShoppingLists.Models.Discount>
{
    public Domain.ShoppingLists.Models.Discount ToDomain(Discount source)
    {
        return new(
            new ItemId(source.ItemId),
            source.ItemTypeId is null ? null : new ItemTypeId(source.ItemTypeId.Value),
            new Price(source.DiscountPrice));
    }
}