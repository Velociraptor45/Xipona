using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemAvailabilityConverter :
        IToDomainConverter<StoreItemAvailabilityContract, StoreItemAvailability>
    {
        private readonly IToDomainConverter<StoreItemStoreContract, StoreItemStore> storeConverter;
        private readonly IToDomainConverter<StoreItemSectionContract, StoreItemSection> sectionConverter;

        public ItemAvailabilityConverter(
            IToDomainConverter<StoreItemStoreContract, StoreItemStore> storeConverter,
            IToDomainConverter<StoreItemSectionContract, StoreItemSection> sectionConverter)
        {
            this.storeConverter = storeConverter;
            this.sectionConverter = sectionConverter;
        }

        public StoreItemAvailability ToDomain(StoreItemAvailabilityContract source)
        {
            return new StoreItemAvailability(
                storeConverter.ToDomain(source.Store),
                source.Price,
                sectionConverter.ToDomain(source.DefaultSection));
        }
    }
}