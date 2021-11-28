using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class ItemTypeFactory : IItemTypeFactory
    {
        public IItemType Create(ItemTypeId id, string name, IEnumerable<IStoreItemAvailability> availabilities)
        {
            return new ItemType(id, name, availabilities);
        }
    }
}