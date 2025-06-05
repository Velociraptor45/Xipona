using Xipona.Api.ApplicationServices.Items.Commands;
using Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

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