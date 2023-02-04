using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresOverview
{
    public class StoreSearchResultContract
    {
        public StoreSearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}