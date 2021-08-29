using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Core.TestKit;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models
{
    public class StoreItemAvailabilityBuilder : TestBuilderBase<StoreItemAvailability>
    {
        public StoreItemAvailabilityBuilder WithStoreId(StoreId storeId)
        {
            FillContructorWith("storeId", storeId);
            return this;
        }

        public StoreItemAvailabilityBuilder WithPrice(float price)
        {
            FillContructorWith("price", price);
            return this;
        }

        public StoreItemAvailabilityBuilder WithDefaultSectionId(SectionId defaultSectionId)
        {
            FillContructorWith("defaultSectionId", defaultSectionId);
            return this;
        }
    }
}