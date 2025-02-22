using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared
{
    /// <summary>
    /// Represents an item type.
    /// </summary>
    public class ItemTypeContract
    {
        /// <summary>
        /// ID of the item type.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the item type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All availabilities of the item type.
        /// </summary>
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}