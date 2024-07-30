using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    /// <summary>
    /// Represents an item type.
    /// </summary>
    public class ItemTypeContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="availabilities"></param>
        public ItemTypeContract(Guid id, string name, IEnumerable<ItemAvailabilityContract> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities;
        }

        /// <summary>
        /// The ID of the item type.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the item type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The availabilities of the item type.
        /// </summary>
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; }
    }
}