using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListItemDefinition
    {
        public ItemId Id { get; set; }
        public ItemCategoryId ItemCategoryId { get; set; }
        public ManufacturerId ManufacturerId { get; set; }
        public bool? IsInBasket { get; set; }
        public float? Quantity { get; set; }

        public ShoppingListItemDefinition Clone()
        {
            return new ShoppingListItemDefinition
            {
                Id = Id,
                ItemCategoryId = ItemCategoryId,
                ManufacturerId = ManufacturerId,
                IsInBasket = IsInBasket,
                Quantity = Quantity
            };
        }

        public static ShoppingListItemDefinition FromId(int id)
        {
            return FromId(new ItemId(id));
        }

        public static ShoppingListItemDefinition FromId(ItemId id)
        {
            return new ShoppingListItemDefinition
            {
                Id = id
            };
        }
    }
}