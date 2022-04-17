using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class ItemType
    {
        public ItemType(Guid id, string name, IEnumerable<ItemAvailability> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities.ToList();
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public List<ItemAvailability> Availabilities { get; }

        public IEnumerable<ItemStore> GetNotRegisteredStores(IEnumerable<Store> stores)
        {
            var registeredStoreIds = Availabilities.Select(av => av.Store.Id).OrderBy(id => id);
            var allStoreIds = stores.Select(s => s.Id).OrderBy(id => id).ToList();

            if (allStoreIds.SequenceEqual(registeredStoreIds))
                return Enumerable.Empty<ItemStore>();

            var availableStoreIds = allStoreIds.Except(registeredStoreIds).ToList();
            return stores.Where(s => availableStoreIds.Contains(s.Id)).Select(s => s.AsStoreItemStore());
        }
    }
}