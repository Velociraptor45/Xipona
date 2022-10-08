using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

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