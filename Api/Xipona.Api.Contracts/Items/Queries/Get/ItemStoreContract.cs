using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    /// <summary>
    /// Represents a store where an item is available.
    /// </summary>
    public class ItemStoreContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sections"></param>
        public ItemStoreContract(Guid id, string name, IEnumerable<ItemSectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        /// <summary>
        /// The ID of the store.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the store.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The sections inside the store.
        /// </summary>
        public IReadOnlyCollection<ItemSectionContract> Sections { get; }
    }
}