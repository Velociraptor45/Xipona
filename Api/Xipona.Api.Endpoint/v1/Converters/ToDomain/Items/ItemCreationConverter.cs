using Xipona.Api.Contracts.Items.Commands.CreateItem;
using Xipona.Api.Contracts.Items.Commands.Shared;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Creations;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class ItemCreationConverter : IToDomainConverter<CreateItemContract, ItemCreation>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, ItemAvailability> _itemAvailabilityConverter;

    public ItemCreationConverter(
        IToDomainConverter<ItemAvailabilityContract, ItemAvailability> itemAvailabilityConverter)
    {
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public ItemCreation ToDomain(CreateItemContract source)
    {
        ItemQuantityInPacket? itemQuantityInPacket = null;
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
            _itemAvailabilityConverter.ToDomain(source.Availabilities));
    }
}