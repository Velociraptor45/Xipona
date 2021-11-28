using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IItemType
    {
        ItemTypeId Id { get; }
        string Name { get; }
        IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
    }
}