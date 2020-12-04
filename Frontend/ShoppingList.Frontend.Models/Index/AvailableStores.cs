using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index
{
    public class AvailableStores
    {
        private readonly IEnumerable<Store> stores;
        private int selectedStoreIndex;

        public AvailableStores(IEnumerable<Store> stores, int selectedStoreIndex = 0)
        {
            this.stores = stores ?? throw new ArgumentNullException(nameof(stores));

            if (selectedStoreIndex < 0)
                throw new ArgumentException($"{nameof(selectedStoreIndex)} mustn't be negative");
            if (stores.Any() && selectedStoreIndex >= stores.ToList().Count)
                throw new ArgumentException($"{nameof(selectedStoreIndex)} is bigger than store count");

            this.selectedStoreIndex = selectedStoreIndex;
        }

        public IReadOnlyCollection<Store> Stores => stores.ToList().AsReadOnly();
        public Store SelectedStore => stores.Any() ? stores.ElementAt(selectedStoreIndex) : null;

        public void SetSelectedStore(int storeId)
        {
            int index = stores.ToList().FindIndex(store => store.Id == storeId);
            if (index == -1)
                throw new ArgumentException("No store with given id found.");

            selectedStoreIndex = index;
        }
    }
}