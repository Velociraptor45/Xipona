using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes
{
    /// <summary>
    /// Represents a command to update an item type.
    /// </summary>
    public class UpdateItemTypeContract
    {
        /// <summary>
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="name"></param>
        /// <param name="availabilities"></param>
        public UpdateItemTypeContract(Guid oldId, string name, IEnumerable<ItemAvailabilityContract> availabilities)
        {
            OldId = oldId;
            Name = name;
            Availabilities = availabilities;
        }

        /// <summary>
        /// The current ID of the item type.
        /// </summary>
        public Guid OldId { get; set; }

        /// <summary>
        /// The new name of the item type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All availabilities of the item type.
        /// </summary>
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}