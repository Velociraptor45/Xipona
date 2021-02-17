using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListItemGenerationDefinition
    {
        public ShoppingListItemId Id { get; set; }
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
        public string Comment { get; set; }
        public bool? IsTemporary { get; set; }
        public float? PricePerQuantity { get; set; }
        public QuantityType? QuantityType { get; set; }
        public float? QuantityInPacket { get; set; }
        public QuantityTypeInPacket? QuantityTypeInPacket { get; set; }
        public IItemCategory ItemCategory { get; set; }
        public IManufacturer Manufacturer { get; set; }
        public bool? IsInBasket { get; set; }
        public float? Quantity { get; set; }
    }
}