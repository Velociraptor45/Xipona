using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemAvailabilityConverter :
        IToDomainConverter<StoreItemAvailabilityContract, ItemAvailability>
    {
        private readonly IToDomainConverter<StoreItemStoreContract, ItemStore> storeConverter;

        public ItemAvailabilityConverter(IToDomainConverter<StoreItemStoreContract, ItemStore> storeConverter)
        {
            this.storeConverter = storeConverter;
        }

        public ItemAvailability ToDomain(StoreItemAvailabilityContract source)
        {
            return new ItemAvailability(
                storeConverter.ToDomain(source.Store),
                source.Price,
                source.DefaultSection.Id);
        }
    }
}