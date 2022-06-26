using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemAvailabilityConverter :
        IToDomainConverter<StoreItemAvailabilityContract, ItemAvailability>
    {
        private readonly IToDomainConverter<StoreItemStoreContract, ItemStore> _storeConverter;

        public ItemAvailabilityConverter(IToDomainConverter<StoreItemStoreContract, ItemStore> storeConverter)
        {
            _storeConverter = storeConverter;
        }

        public ItemAvailability ToDomain(StoreItemAvailabilityContract source)
        {
            return new ItemAvailability(
                _storeConverter.ToDomain(source.Store),
                source.Price,
                source.DefaultSection.Id);
        }
    }
}