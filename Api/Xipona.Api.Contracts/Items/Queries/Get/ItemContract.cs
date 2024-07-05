using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    /// <summary>
    /// Represents an item.
    /// </summary>
    public class ItemContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isDeleted"></param>
        /// <param name="comment"></param>
        /// <param name="isTemporary"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantityInPacket"></param>
        /// <param name="quantityTypeInPacket"></param>
        /// <param name="itemCategory"></param>
        /// <param name="manufacturer"></param>
        /// <param name="availabilities"></param>
        /// <param name="itemTypes"></param>
        public ItemContract(Guid id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityTypeContract quantityType, float? quantityInPacket,
            QuantityTypeInPacketContract quantityTypeInPacket, ItemCategoryContract itemCategory,
            ManufacturerContract manufacturer, IEnumerable<ItemAvailabilityContract> availabilities,
            IEnumerable<ItemTypeContract> itemTypes)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            Availabilities = availabilities;
            ItemTypes = itemTypes;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether the item is deleted. True if the item is deleted, otherwise false.
        /// </summary>
        public bool IsDeleted { get; }

        /// <summary>
        /// The custom comment attached to the item.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Whether the item is temporary. True if the item is temporary, otherwise false.
        /// </summary>
        public bool IsTemporary { get; }

        /// <summary>
        /// The quantity type of the item.
        /// </summary>
        public QuantityTypeContract QuantityType { get; }

        /// <summary>
        /// The quantity in the item's packet. Null if the item doesn't have a packet.
        /// </summary>
        public float? QuantityInPacket { get; }

        /// <summary>
        /// The quantity type of the item's packet. Null if the item doesn't have a packet.
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
        /// The availabilities of the item. Empty if the item has types.
        /// </summary>
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; }

        /// <summary>
        /// The possible types of the item. Empty if the item doesn't have any types.
        /// </summary>
        public IEnumerable<ItemTypeContract> ItemTypes { get; }
    }
}