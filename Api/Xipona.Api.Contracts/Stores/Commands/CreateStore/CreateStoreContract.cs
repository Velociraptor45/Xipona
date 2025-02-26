﻿using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore
{
    /// <summary>
    /// Represents a request for creating a store.
    /// </summary>
    public class CreateStoreContract
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sections"></param>
        public CreateStoreContract(string name, IEnumerable<CreateSectionContract> sections)
        {
            Name = name;
            Sections = sections;
        }

        /// <summary>
        /// The name of the store.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sections of the store.
        /// </summary>
        public IEnumerable<CreateSectionContract> Sections { get; set; }
    }
}