using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemAvailability : IStoreItemAvailability
    {
        public StoreItemAvailability(IStoreItemStore store, float price, StoreItemSectionId defaultSectionId)
        {
            Store = store ?? throw new System.ArgumentNullException(nameof(store));
            Price = price;
            DefaultSection = store.Sections.Single(s => s.Id == defaultSectionId); //todo add throwing domain exception
        }

        public IStoreItemStore Store { get; }
        public float Price { get; }
        public IStoreItemSection DefaultSection { get; }
    }
}