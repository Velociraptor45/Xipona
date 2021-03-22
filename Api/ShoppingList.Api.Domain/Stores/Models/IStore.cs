using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public interface IStore
    {
        StoreId Id { get; }
        string Name { get; }
        bool IsDeleted { get; }
        IReadOnlyCollection<IStoreSection> Sections { get; }

        void ChangeName(string name);
        void UpdateStores(IEnumerable<IStoreSection> storeSections);
    }
}