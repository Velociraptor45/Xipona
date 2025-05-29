using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveShoppingListDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveShoppingListDiscount;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class RemoveShoppingListDiscountCommandConverter
    : IToDomainConverter<(Guid, RemoveShoppingListDiscountContract), RemoveShoppingListDiscountCommand>
{
    public RemoveShoppingListDiscountCommand ToDomain((Guid, RemoveShoppingListDiscountContract) source)
    {
        return new(
            new ShoppingListId(source.Item1),
            new ListDiscountId(source.Item2.DiscountId));
    }
}
