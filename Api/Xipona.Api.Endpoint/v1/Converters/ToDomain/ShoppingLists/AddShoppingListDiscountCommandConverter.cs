using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddShoppingListDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddShoppingListDiscount;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class AddShoppingListDiscountCommandConverter
    : IToDomainConverter<(Guid, AddShoppingListDiscountContract), AddShoppingListDiscountCommand>
{
    public AddShoppingListDiscountCommand ToDomain((Guid, AddShoppingListDiscountContract) source)
    {
        var discount = source.Item2.DiscountPrice is null
            ? new ListDiscount(ListDiscountId.New, new Percentage(source.Item2.DiscountPercentage!.Value))
            : new ListDiscount(ListDiscountId.New, source.Item2.DiscountPrice.Value);

        return new(
            new ShoppingListId(source.Item1),
            discount);
    }
}
