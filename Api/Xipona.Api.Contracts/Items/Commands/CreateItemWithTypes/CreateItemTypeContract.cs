using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes
{
    /// <summary>
    /// Represents a command to create an item type.
    /// </summary>
    public class CreateItemTypeContract
    {
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