using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using System;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class ModifyItemTypeContractConverter : IToContractConverter<EditedItemType, ModifyItemTypeContract>
{
    private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

    public ModifyItemTypeContractConverter(
        IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public ModifyItemTypeContract ToContract(EditedItemType source)
    {
        return new ModifyItemTypeContract
        {
            Id = source.Id == Guid.Empty ? null : source.Id,
            Name = source.Name,
            Availabilities = source.Availabilities.Select(av => _availabilityConverter.ToContract(av))
        };
    }
}