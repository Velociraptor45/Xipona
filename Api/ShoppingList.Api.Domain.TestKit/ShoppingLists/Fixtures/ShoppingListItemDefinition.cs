using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListItemDefinition
    {
        public ItemId Id { get; set; }
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

        public ShoppingListItemDefinition Clone()
        {
            return new ShoppingListItemDefinition
            {
                Id = Id,
                Name = Name,
                IsDeleted = IsDeleted,
                Comment = Comment,
                IsTemporary = IsTemporary,
                PricePerQuantity = PricePerQuantity,
                QuantityType = QuantityType,
                QuantityInPacket = QuantityInPacket,
                QuantityTypeInPacket = QuantityTypeInPacket,
                ItemCategory = ItemCategory,
                Manufacturer = Manufacturer,
                IsInBasket = IsInBasket,
                Quantity = Quantity
            };
        }

        public static ShoppingListItemDefinition FromId(int id)
        {
            return FromId(new ItemId(id));
        }

        public static ShoppingListItemDefinition FromId(Guid id)
        {
            return FromId(new ShoppingListItemId(id));
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