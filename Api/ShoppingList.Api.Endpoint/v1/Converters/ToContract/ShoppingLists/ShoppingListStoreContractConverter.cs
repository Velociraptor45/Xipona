using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class ShoppingListStoreContractConverter :
    IToContractConverter<ShoppingListStoreReadModel, ShoppingListStoreContract>
{
    public ShoppingListStoreContract ToContract(ShoppingListStoreReadModel source)
    {
        return new ShoppingListStoreContract(source.Id.Value, source.Name);
    }
}