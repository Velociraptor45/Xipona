namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States
{
    public interface IAvailable
    {
        IReadOnlyCollection<EditedItemAvailability> Availabilities { get; }

        IEnumerable<ItemStore> GetNotRegisteredStores(IEnumerable<ItemStore> stores)
        {
            var storesList = stores.ToList();

            var registeredStoreIds = Availabilities.Select(av => av.StoreId).OrderBy(id => id).ToList();
            var allStoreIds = storesList.Select(s => s.Id).OrderBy(id => id).ToList();

            if (allStoreIds.SequenceEqual(registeredStoreIds))
                return Enumerable.Empty<ItemStore>();

            var availableStoreIds = allStoreIds.Except(registeredStoreIds).ToList();
            return storesList.Where(s => availableStoreIds.Contains(s.Id));
        }

        IEnumerable<EditedItemAvailabilityStore> CreateAvailabilityStores(
            EditedItemAvailability availability, IEnumerable<ItemStore> stores)
        {
            var allOccupiedStoreIds = Availabilities.Select(av => av.StoreId).ToArray();

            var avStores = stores
                .Select(s =>
                {
                    var isDisabled = s.Id != availability.StoreId && allOccupiedStoreIds.Contains(s.Id);
                    return new EditedItemAvailabilityStore(s.Id, s.Name, isDisabled);
                })
                .ToList();
            return avStores.OrderBy(s => s.Name);
        }
    }
}