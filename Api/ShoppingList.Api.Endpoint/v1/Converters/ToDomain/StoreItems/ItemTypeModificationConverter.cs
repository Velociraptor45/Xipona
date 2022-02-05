using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class ItemTypeModificationConverter : IToDomainConverter<ItemTypeContract, ItemTypeModification>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> _availabilityConverter;

    public ItemTypeModificationConverter(
        IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public ItemTypeModification ToDomain(ItemTypeContract source)
    {
        return new ItemTypeModification(
            new ItemTypeId(source.Id),
            source.Name,
            _availabilityConverter.ToDomain(source.Availabilities));
    }
}