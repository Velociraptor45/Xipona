using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract;

public class UpdateItemPriceContractConverter : IToContractConverter<UpdateItemPriceRequest, UpdateItemPriceContract>
{
    public UpdateItemPriceContract ToContract(UpdateItemPriceRequest source)
    {
        return new UpdateItemPriceContract(source.ItemTypeId, source.StoreId, source.Price);
    }
}