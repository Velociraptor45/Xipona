using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class CreateItemContractConverter :
    IToContractConverter<EditedItem, CreateItemContract>
{
    private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

    public CreateItemContractConverter(
        IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public CreateItemContract ToContract(EditedItem source)
    {
        return new CreateItemContract
        {
            Name = source.Name,
            Comment = source.Comment,
            QuantityType = source.QuantityType.Id,
            QuantityInPacket = source.QuantityInPacket,
            QuantityTypeInPacket = source.QuantityInPacketType?.Id,
            ItemCategoryId = source.ItemCategoryId.Value,
            ManufacturerId = source.ManufacturerId,
            Availabilities = source.Availabilities.Select(_availabilityConverter.ToContract)
        };
    }
}