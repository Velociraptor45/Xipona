using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemStoreFactory
    {
        IStoreItemStore Create(IStore store);
        IStoreItemStore Create(StoreItemStoreId id, string name, IEnumerable<IStoreItemSection> sections);
    }
}