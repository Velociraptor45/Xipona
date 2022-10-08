using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class ItemAvailabilityConverter :
        IToDomainConverter<ItemAvailabilityContract, ItemAvailability>
    {
        private readonly IToDomainConverter<ItemStoreContract, ItemStore> _storeConverter;

        public ItemAvailabilityConverter(IToDomainConverter<ItemStoreContract, ItemStore> storeConverter)
        {
            _storeConverter = storeConverter;
        }

        public ItemAvailability ToDomain(ItemAvailabilityContract source)
        {
            return new ItemAvailability(
                _storeConverter.ToDomain(source.Store),
                source.Price,
                source.DefaultSection.Id);
        }
    }
}