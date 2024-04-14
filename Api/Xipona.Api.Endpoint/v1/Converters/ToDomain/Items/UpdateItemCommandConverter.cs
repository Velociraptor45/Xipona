using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class UpdateItemCommandConverter : IToDomainConverter<(Guid id, UpdateItemContract contract), UpdateItemCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, ItemAvailability> _itemAvailabilityConverter;

    public UpdateItemCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, ItemAvailability> itemAvailabilityConverter)
    {
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public UpdateItemCommand ToDomain((Guid id, UpdateItemContract contract) source)
    {
        var (id, contract) = source;

        ItemQuantityInPacket? itemQuantityInPacket = null;
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(contract.QuantityInPacket.Value),
                contract.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        var update = new ItemUpdate(
            new ItemId(id),
            new ItemName(contract.Name),
            new Comment(contract.Comment),
            new ItemQuantity(
                contract.QuantityType.ToEnum<QuantityType>(),
                itemQuantityInPacket),
            new ItemCategoryId(contract.ItemCategoryId),
            contract.ManufacturerId.HasValue ?
                new ManufacturerId(contract.ManufacturerId.Value) :
                null,
            _itemAvailabilityConverter.ToDomain(contract.Availabilities));

        return new UpdateItemCommand(update);
    }
}