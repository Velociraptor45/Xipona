using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Items;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class UpdateItemPriceContractConverter : IToContractConverter<UpdateItemPriceRequest, UpdateItemPriceContract>
{
    public UpdateItemPriceContract ToContract(UpdateItemPriceRequest source)
    {
        return new UpdateItemPriceContract(source.ItemTypeId, source.StoreId, source.Price);
    }
}