using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class ShoppingListStoreContractConverter :
    IToContractConverter<ShoppingListStoreReadModel, ShoppingListStoreContract>
{
    public ShoppingListStoreContract ToContract(ShoppingListStoreReadModel source)
    {
        return new ShoppingListStoreContract(source.Id, source.Name);
    }
}