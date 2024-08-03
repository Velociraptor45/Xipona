using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore
{
    /// <summary>
    /// Represents a request for modifying a store.
    /// </summary>
    public class ModifyStoreContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sections"></param>
        public ModifyStoreContract(Guid id, string name, IEnumerable<ModifySectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections;
        }

        /// <summary>
        /// The ID of the store.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the store.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sections of the store.
        /// </summary>
        public IEnumerable<ModifySectionContract> Sections { get; set; }
    }
}