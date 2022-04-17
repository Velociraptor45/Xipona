using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Stores.Models
{
    public class AvailableStores
    {
        public AvailableStores(IEnumerable<Store> stores, Guid selectedStoreId)
        {
            Stores = stores?.ToList() ?? throw new ArgumentNullException(nameof(stores));
            SelectedStoreId = selectedStoreId;
        }

        public List<Store> Stores { get; }
        public Guid SelectedStoreId { get; set; }
        public Store SelectedStore => Stores.Single(s => s.Id == SelectedStoreId);
    }
}