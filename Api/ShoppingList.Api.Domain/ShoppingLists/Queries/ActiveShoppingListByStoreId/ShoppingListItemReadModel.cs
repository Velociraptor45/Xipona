using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId
{
    public class ShoppingListItemReadModel
    {
        public ShoppingListItemReadModel(ItemId id, ItemTypeId? typeId, string name, bool isDeleted, string comment,
            bool isTemporary, float pricePerQuantity, QuantityTypeReadModel quantityType, float quantityInPacket,
            QuantityTypeInPacketReadModel quantityTypeInPacket,
            ItemCategoryReadModel? itemCategory, ManufacturerReadModel? manufacturer,
            bool isInBasket, float quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Id = id;
            TypeId = typeId;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType ?? throw new ArgumentNullException(nameof(quantityType));
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket ?? throw new ArgumentNullException(nameof(quantityTypeInPacket));
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public ItemId Id { get; }
        public ItemTypeId? TypeId { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityTypeReadModel QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacketReadModel QuantityTypeInPacket { get; }
        public ItemCategoryReadModel? ItemCategory { get; }
        public ManufacturerReadModel? Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}