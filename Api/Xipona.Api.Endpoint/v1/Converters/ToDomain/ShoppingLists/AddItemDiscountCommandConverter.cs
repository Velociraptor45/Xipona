using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class AddItemDiscountCommandConverter : IToDomainConverter<(Guid, AddItemDiscountContract), AddItemDiscountCommand>
{
    public AddItemDiscountCommand ToDomain((Guid, AddItemDiscountContract) source)
    {
        var itemTypeId = source.Item2.ItemTypeId;
        var discount = new Discount(
            new ItemId(source.Item2.ItemId),
            itemTypeId is null ? null : new ItemTypeId(itemTypeId.Value),
            new Price(source.Item2.DiscountPrice));

        return new(new ShoppingListId(source.Item1), discount);
    }
}
