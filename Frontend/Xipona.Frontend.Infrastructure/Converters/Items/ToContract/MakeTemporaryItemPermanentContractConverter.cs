using ProjectHermes.Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Items;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class MakeTemporaryItemPermanentContractConverter :
    IToContractConverter<MakeTemporaryItemPermanentRequest, MakeTemporaryItemPermanentContract>
{
    private readonly IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> _availabilityConverter;

    public MakeTemporaryItemPermanentContractConverter(
        IToContractConverter<EditedItemAvailability, ItemAvailabilityContract> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public MakeTemporaryItemPermanentContract ToContract(MakeTemporaryItemPermanentRequest source)
    {
        return new MakeTemporaryItemPermanentContract(
            source.Name,
            source.Comment,
            source.QuantityType,
            source.QuantityInPacket,
            source.QuantityTypeInPacket,
            source.ItemCategoryId,
            source.ManufacturerId,
            source.Availabilities.Select(_availabilityConverter.ToContract));
    }
}