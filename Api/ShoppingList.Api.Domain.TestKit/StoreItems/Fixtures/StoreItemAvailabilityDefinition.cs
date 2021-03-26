using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures
{
    public class StoreItemAvailabilityDefinition
    {
        public StoreId StoreId { get; set; }
        public float? Price { get; set; }
        public SectionId DefaultSectionId { get; set; }
    }
}