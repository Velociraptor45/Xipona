using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class ModifyItemContractConverter : IToContractConverter<EditedItem, ModifyItemContract>
{
    private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

    public ModifyItemContractConverter(
        IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public ModifyItemContract ToContract(EditedItem source)
    {
        return new ModifyItemContract(
            source.Name,
            source.Comment,
            source.QuantityType.Id,
            source.QuantityInPacket,
            source.QuantityInPacketType?.Id,
            source.ItemCategoryId.Value,
            source.ManufacturerId,
            source.Availabilities.Select(_availabilityConverter.ToContract));
    }
}