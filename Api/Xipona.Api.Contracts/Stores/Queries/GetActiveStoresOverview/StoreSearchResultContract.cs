using System;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview
{
    /// <summary>
    /// Represents the search result of a store.
    /// </summary>
    public class StoreSearchResultContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public StoreSearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// The ID of the store.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the store.
        /// </summary>
        public string Name { get; }
    }
}