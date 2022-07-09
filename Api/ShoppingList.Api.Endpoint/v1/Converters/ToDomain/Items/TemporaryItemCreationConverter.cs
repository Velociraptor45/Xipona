using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

public class TemporaryItemCreationConverter : IToDomainConverter<CreateTemporaryItemContract, TemporaryItemCreation>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _itemAvailabilityConverter;

    public TemporaryItemCreationConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> itemAvailabilityConverter)
    {
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public TemporaryItemCreation ToDomain(CreateTemporaryItemContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new TemporaryItemCreation(
            source.ClientSideId,
            new ItemName(source.Name),
            _itemAvailabilityConverter.ToDomain(source.Availability));
    }
}