using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToDomain;

public class ShoppingListItemConverter : IToDomainConverter<ItemsOnList, IShoppingListItem>
{
    private readonly IShoppingListItemFactory _shoppingListItemFactory;

    public ShoppingListItemConverter(IShoppingListItemFactory shoppingListItemFactory)
    {
        _shoppingListItemFactory = shoppingListItemFactory;
    }

    public IShoppingListItem ToDomain(ItemsOnList source)
    {
        var itemTypeId = source.ItemTypeId.HasValue ? new ItemTypeId(source.ItemTypeId.Value) : (ItemTypeId?)null;

        return _shoppingListItemFactory.Create(
            new ItemId(source.ItemId),
            itemTypeId,
            source.InBasket,
            new QuantityInBasket(source.Quantity));
    }
}