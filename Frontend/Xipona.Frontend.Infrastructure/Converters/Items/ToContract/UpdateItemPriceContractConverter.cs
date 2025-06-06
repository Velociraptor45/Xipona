using Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.Items;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class UpdateItemPriceContractConverter : IToContractConverter<UpdateItemPriceRequest, UpdateItemPriceContract>
{
    public UpdateItemPriceContract ToContract(UpdateItemPriceRequest source)
    {
        return new UpdateItemPriceContract(source.ItemTypeId, source.StoreId, source.Price);
    }
}