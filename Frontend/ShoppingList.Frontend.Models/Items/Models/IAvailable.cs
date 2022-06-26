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
            var storesList = stores.ToList();

            var registeredStoreIds = Availabilities.Select(av => av.Store.Id).OrderBy(id => id).ToList();
            var allStoreIds = storesList.Select(s => s.Id).OrderBy(id => id).ToList();

            if (allStoreIds.SequenceEqual(registeredStoreIds))
                return Enumerable.Empty<ItemStore>();

            var availableStoreIds = allStoreIds.Except(registeredStoreIds).ToList();
            return storesList.Where(s => availableStoreIds.Contains(s.Id)).Select(s => s.AsItemStore());
        }

        bool AddStore(IEnumerable<Store> stores)
        {
            var storesList = stores.ToList();

            var availableStores = GetNotRegisteredStores(storesList).ToList();
            if (!availableStores.Any())
                return false;

            var chosenStore = availableStores.First();
            var section = storesList
                .Single(s => s.Id == chosenStore.Id)
                .Sections
                .Single(s => s.IsDefaultSection)
                .AsItemSection();

            Availabilities.Add(new ItemAvailability(chosenStore, 1, section.Id));
            return true;
        }
    }
}