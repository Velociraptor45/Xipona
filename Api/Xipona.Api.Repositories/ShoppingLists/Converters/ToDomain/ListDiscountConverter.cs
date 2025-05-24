using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;

public class ListDiscountConverter : IToDomainConverter<ShoppingListDiscount, ListDiscount>
{
    public ListDiscount ToDomain(ShoppingListDiscount source)
    {
        if (source.DiscountPercentage is not null)
            return new(new(source.DiscountPercentage.Value));
        if (source.DiscountPrice is not null)
            return new(source.DiscountPrice.Value);

        throw new InvalidOperationException($"Discount {source.Id} does not contain a price or percentage");
    }
}