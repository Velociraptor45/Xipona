using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class ItemType
    {
        public ItemType(int id, string name, IEnumerable<StoreItemAvailability> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities.ToList();
        }

        public int Id { get; }
        public string Name { get; set; }
        public List<StoreItemAvailability> Availabilities { get; }

        public IEnumerable<StoreItemStore> GetNotRegisteredStores(IEnumerable<Store> stores)
        {
            var registeredStoreIds = Availabilities.Select(av => av.Store.Id).OrderBy(id => id);
            var allStoreIds = stores.Select(s => s.Id).OrderBy(id => id);

            if (allStoreIds.SequenceEqual(registeredStoreIds))
                return Enumerable.Empty<StoreItemStore>();

            var availableStoreIds = allStoreIds.Except(registeredStoreIds).ToList();
            return stores.Where(s => availableStoreIds.Contains(s.Id)).Select(s => s.AsStoreItemStore());
        }
    }
}