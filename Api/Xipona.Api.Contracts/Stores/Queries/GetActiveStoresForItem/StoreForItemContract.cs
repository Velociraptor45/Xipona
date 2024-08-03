using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem
{
    /// <summary>
    /// Represents a store.
    /// </summary>
    public class StoreForItemContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sections"></param>
        public StoreForItemContract(Guid id, string name, IEnumerable<SectionForItemContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections;
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
        /// The sections of the store.
        /// </summary>
        public IEnumerable<SectionForItemContract> Sections { get; }
    }
}