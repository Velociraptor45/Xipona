using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItem
{
    /// <summary>
    /// Represents a command to create an item.
    /// </summary>
    public class CreateItemContract
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The comment added to the item.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The quantity type of the item.
        /// </summary>
        public int QuantityType { get; set; }

        /// <summary>
        /// The in packet quantity of the item. Only valid to set if <see cref="QuantityType"/> is 0 (Unit).
        /// </summary>
        public float? QuantityInPacket { get; set; }

        /// <summary>
        /// The quantity type in packet of the item. Only valid to set if <see cref="QuantityType"/> is 0 (Unit).
        /// </summary>
        public int? QuantityTypeInPacket { get; set; }

        /// <summary>
        /// The ID of the item's item category.
        /// </summary>
        public Guid ItemCategoryId { get; set; }

        /// <summary>
        /// The ID of the item's manufacturer.
        /// </summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>
        /// All availabilities of the item.
        /// </summary>
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}