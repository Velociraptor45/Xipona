using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId
{
    /// <summary>
    /// Represents a store.
    /// </summary>
    public class ShoppingListStoreContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ShoppingListStoreContract(Guid id, string name)
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