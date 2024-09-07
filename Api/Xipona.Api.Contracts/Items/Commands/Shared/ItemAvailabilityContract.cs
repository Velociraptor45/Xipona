using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared
{
    /// <summary>
    /// Represents an item or item type's availability.
    /// </summary>
    public class ItemAvailabilityContract
    {
        /// <summary>
        /// The ID of the store where the item is available.
        /// </summary>
        public Guid StoreId { get; set; }

        /// <summary>
        /// The price of the item in the store.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The ID of the section where the item is located in the store.
        /// </summary>
        public Guid DefaultSectionId { get; set; }
    }
}