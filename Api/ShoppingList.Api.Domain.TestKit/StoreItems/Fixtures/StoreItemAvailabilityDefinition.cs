using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemAvailabilityDefinition
    {
        public IStoreItemStore Store { get; set; }
        public float? Price { get; set; }
        public StoreItemSectionId DefaultSectionId { get; set; }
    }
}