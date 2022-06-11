using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public interface IAvailable
    {
        List<ItemAvailability> Availabilities { get; }

        IEnumerable<ItemStore> GetNotRegisteredStores(IEnumerable<Store> stores)
        {
            var registeredStoreIds = Availabilities.Select(av => av.Store.Id).OrderBy(id => id);
            var allStoreIds = stores.Select(s => s.Id).OrderBy(id => id).ToList();

            if (allStoreIds.SequenceEqual(registeredStoreIds))
                return Enumerable.Empty<ItemStore>();

            var availableStoreIds = allStoreIds.Except(registeredStoreIds).ToList();
            return stores.Where(s => availableStoreIds.Contains(s.Id)).Select(s => s.AsItemStore());
        }
    }
}