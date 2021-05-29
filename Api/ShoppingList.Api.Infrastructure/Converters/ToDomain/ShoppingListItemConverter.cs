using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class ShoppingListItemConverter : IToDomainConverter<ItemsOnList, IShoppingListItem>
    {
        private readonly IShoppingListItemFactory shoppingListItemFactory;

        public ShoppingListItemConverter(IShoppingListItemFactory shoppingListItemFactory)
        {
            this.shoppingListItemFactory = shoppingListItemFactory;
        }

        public IShoppingListItem ToDomain(ItemsOnList source)
        {
            return shoppingListItemFactory.Create(
                new ItemId(source.ItemId),
                source.InBasket,
                source.Quantity);
        }
    }
}