using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class ItemTypeConverter : IToDomainConverter<ItemTypeContract, IItemType>
    {
        private readonly IItemTypeFactory _itemTypeFactory;
        private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> _availabilityConverter;

        public ItemTypeConverter(IItemTypeFactory itemTypeFactory,
            IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> availabilityConverter)
        {
            _itemTypeFactory = itemTypeFactory;
            _availabilityConverter = availabilityConverter;
        }

        public IItemType ToDomain(ItemTypeContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return _itemTypeFactory.Create(
                new ItemTypeId(source.Id),
                source.Name,
                _availabilityConverter.ToDomain(source.Availabilities));
        }
    }
}