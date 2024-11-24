using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId
{
    /// <summary>
    /// Represents a shopping list item.
    /// </summary>
    public class ShoppingListItemContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeId"></param>
        /// <param name="name"></param>
        /// <param name="isDeleted"></param>
        /// <param name="comment"></param>
        /// <param name="isTemporary"></param>
        /// <param name="pricePerQuantity"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantityInPacket"></param>
        /// <param name="quantityTypeInPacket"></param>
        /// <param name="itemCategory"></param>
        /// <param name="manufacturer"></param>
        /// <param name="isInBasket"></param>
        /// <param name="quantity"></param> 
        /// <param name="isDiscounted"></param> 
        public ShoppingListItemContract(Guid id, Guid? typeId, string name, bool isDeleted, string comment, bool isTemporary,
            decimal pricePerQuantity, QuantityTypeContract quantityType, float? quantityInPacket,
            QuantityTypeInPacketContract quantityTypeInPacket, ItemCategoryContract itemCategory,
            ManufacturerContract manufacturer, bool isInBasket, float quantity, bool isDiscounted)
        {
            Id = id;
            TypeId = typeId;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
            IsDiscounted = isDiscounted;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The ID of the item type.
        /// Null if the item does not have types.
        /// </summary>
        public Guid? TypeId { get; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether the item is deleted. <c>True</c> if the item is deleted, <c>false</c> otherwise.
        /// </summary>
        public bool IsDeleted { get; }

        /// <summary>
        /// The comment attached to the item.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Whether the item is temporary. <c>True</c> if the item is temporary, <c>false</c> otherwise.
        /// </summary>
        public bool IsTemporary { get; }

        /// <summary>
        /// The price per quantity of the item.
        /// </summary>
        public decimal PricePerQuantity { get; }

        /// <summary>
        /// The quantity type of the item.
        /// </summary>
        public QuantityTypeContract QuantityType { get; }

        /// <summary>
        /// The quantity of the item in a packet.
        /// Null if the item does not come in packets.
        /// </summary>
        public float? QuantityInPacket { get; }

        /// <summary>
        /// The quantity type in the item's packet.
        /// Null if the item does not come in packets.
        /// </summary>
        public QuantityTypeInPacketContract QuantityTypeInPacket { get; }

        /// <summary>
        /// The category of the item.
        /// </summary>
        public ItemCategoryContract ItemCategory { get; }

        /// <summary>
        /// The manufacturer of the item.
        /// </summary>
        public ManufacturerContract Manufacturer { get; }

        /// <summary>
        /// Whether the item is in the basket. <c>True</c> if the item is in the basket, <c>false</c> otherwise.
        /// </summary>
        public bool IsInBasket { get; }

        /// <summary>
        /// The quantity of the item.
        /// </summary>
        public float Quantity { get; }

        /// <summary>
        /// Whether the item is discounted. <c>True</c> if the item is discounted, <c>false</c> otherwise.
        /// </summary>
        public bool IsDiscounted { get; }
    }
}