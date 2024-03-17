using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;

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