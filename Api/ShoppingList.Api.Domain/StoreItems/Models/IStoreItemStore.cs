using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IStoreItemStore
    {
        StoreItemStoreId Id { get; }
        string Name { get; }
        IReadOnlyCollection<IStoreItemSection> Sections { get; }
    }
}