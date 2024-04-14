using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class UpdateItemPriceCommandConverter : IToDomainConverter<(Guid, UpdateItemPriceContract), UpdateItemPriceCommand>
{
    public UpdateItemPriceCommand ToDomain((Guid, UpdateItemPriceContract) source)
    {
        (Guid itemId, UpdateItemPriceContract? contract) = source;

        return new UpdateItemPriceCommand(
            new ItemId(itemId),
            contract.ItemTypeId is null ? null : new ItemTypeId(contract.ItemTypeId.Value),
            new StoreId(contract.StoreId),
            new Price(contract.Price));
    }
}