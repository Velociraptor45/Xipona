using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Repositories.ShoppingLists.Entities;

namespace Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;

public class ShoppingListItemConverter : IToDomainConverter<ItemsOnList, ShoppingListItem>
{
    public ShoppingListItem ToDomain(ItemsOnList source)
    {
        var itemTypeId = source.ItemTypeId.HasValue ? new ItemTypeId(source.ItemTypeId.Value) : (ItemTypeId?)null;

        return new ShoppingListItem(
            new ItemId(source.ItemId),
            itemTypeId,
            source.InBasket,
            new QuantityInBasket(source.Quantity));
    }
}