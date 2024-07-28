using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get
{
    /// <summary>
    /// Represents a store.
    /// </summary>
    public class StoreContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sections"></param>
        public StoreContract(Guid id, string name, IEnumerable<SectionContract> sections)
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
        /// The sections of the store.
        /// </summary>
        public IReadOnlyCollection<SectionContract> Sections { get; }
    }
}