using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListItemContract
    {
        public ShoppingListItemContract(Guid id, Guid? typeId, string name, bool isDeleted, string comment, bool isTemporary,
            float pricePerQuantity, QuantityTypeContract quantityType, float? quantityInPacket,
            QuantityTypeInPacketContract quantityTypeInPacket,
            ItemCategoryContract itemCategory, ManufacturerContract manufacturer, bool isInBasket, float quantity)
        {
            Id = id;
            TypeId = typeId;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType ?? throw new ArgumentNullException(nameof(quantityType));
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public Guid? TypeId { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityTypeContract QuantityType { get; }
        public float? QuantityInPacket { get; }
        public QuantityTypeInPacketContract QuantityTypeInPacket { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}