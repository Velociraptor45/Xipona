using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class ItemType : IItemType
    {
        public ItemType(ItemTypeId id, string name, IEnumerable<IStoreItemAvailability> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities.ToList();
        }

        public ItemTypeId Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
    }
}