using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class TemporaryItemCreationConverter : IToDomainConverter<CreateTemporaryItemContract, TemporaryItemCreation>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _storeItemAvailabilityConverter;

    public TemporaryItemCreationConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> storeItemAvailabilityConverter)
    {
        _storeItemAvailabilityConverter = storeItemAvailabilityConverter;
    }

    public TemporaryItemCreation ToDomain(CreateTemporaryItemContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new TemporaryItemCreation(
            source.ClientSideId,
            new ItemName(source.Name),
            _storeItemAvailabilityConverter.ToDomain(source.Availability));
    }
}