using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class RemoveItemDiscountCommandConverter : IToDomainConverter<(Guid, RemoveItemDiscountContract), RemoveItemDiscountCommand>
{
    public RemoveItemDiscountCommand ToDomain((Guid, RemoveItemDiscountContract) source)
    {
        var itemTypeId = source.Item2.ItemTypeId;
        return new(
            new ShoppingListId(source.Item1),
            new ItemId(source.Item2.ItemId),
            itemTypeId is null ? null : new ItemTypeId(itemTypeId.Value));
    }
}
