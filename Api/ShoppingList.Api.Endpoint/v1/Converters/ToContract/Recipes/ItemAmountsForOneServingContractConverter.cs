using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Recipes;

public class ItemAmountsForOneServingContractConverter
    : IToContractConverter<IEnumerable<ItemAmountForOneServing>, ItemAmountsForOneServingContract>
{
    public ItemAmountsForOneServingContract ToContract(IEnumerable<ItemAmountForOneServing> source)
    {
        return new ItemAmountsForOneServingContract(source.Select(i =>
            new ItemAmountForOneServingContract(
                i.ItemId.Value,
                i.ItemTypeId?.Value,
                i.QuantityType.ToInt(),
                i.QuantityLabel,
                i.Quantity,
                i.DefaultStoreId,
                i.AddToShoppingListByDefault,
                i.Availabilities.Select(av => new ItemAmountForOneServingAvailabilityContract(
                    av.StoreId,
                    av.StoreName,
                    av.Price)))));
    }
}