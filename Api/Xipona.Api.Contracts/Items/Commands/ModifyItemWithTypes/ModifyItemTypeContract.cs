using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes
{
    /// <summary>
    /// Represents a command to modify an item type.
    /// </summary>
    public class ModifyItemTypeContract
    {
        /// <summary>
        /// The ID of the item type. If null, the item type will be created.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The name of the item type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The availabilities of the item type.
        /// </summary>
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}