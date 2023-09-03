using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Converters.ToDomain;

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