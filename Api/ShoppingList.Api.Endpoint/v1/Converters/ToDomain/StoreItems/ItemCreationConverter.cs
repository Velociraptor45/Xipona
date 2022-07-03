using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class ItemCreationConverter : IToDomainConverter<CreateItemContract, ItemCreation>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _storeItemAvailabilityConverter;

    public ItemCreationConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> storeItemAvailabilityConverter)
    {
        _storeItemAvailabilityConverter = storeItemAvailabilityConverter;
    }

    public ItemCreation ToDomain(CreateItemContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        ItemQuantityInPacket? itemQuantityInPacket = null;
        //todo improve this check
        if (source.QuantityInPacket is not null && source.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(source.QuantityInPacket.Value),
                source.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        return new ItemCreation(
            new ItemName(source.Name),
            new Comment(source.Comment),
            new ItemQuantity(
                source.QuantityType.ToEnum<QuantityType>(),
                itemQuantityInPacket),
            new ItemCategoryId(source.ItemCategoryId),
            source.ManufacturerId.HasValue ?
                new ManufacturerId(source.ManufacturerId.Value) :
                null,
            _storeItemAvailabilityConverter.ToDomain(source.Availabilities));
    }
}