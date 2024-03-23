using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class ShoppingListStoreContractConverter :
    IToContractConverter<ShoppingListStoreReadModel, ShoppingListStoreContract>
{
    public ShoppingListStoreContract ToContract(ShoppingListStoreReadModel source)
    {
        return new ShoppingListStoreContract(source.Id, source.Name);
    }
}