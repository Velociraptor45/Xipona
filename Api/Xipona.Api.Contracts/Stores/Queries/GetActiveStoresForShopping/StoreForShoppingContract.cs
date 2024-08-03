using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping
{
    /// <summary>
    /// Represents a store that is available for shopping.
    /// </summary>
    public class StoreForShoppingContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sections"></param>
        public StoreForShoppingContract(Guid id, string name, IEnumerable<SectionForShoppingContract> sections)
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
        public IEnumerable<SectionForShoppingContract> Sections { get; }
    }
}