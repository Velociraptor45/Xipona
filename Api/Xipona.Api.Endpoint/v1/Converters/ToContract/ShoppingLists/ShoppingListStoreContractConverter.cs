using Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;

public class ShoppingListStoreContractConverter :
    IToContractConverter<ShoppingListStoreReadModel, ShoppingListStoreContract>
{
    public ShoppingListStoreContract ToContract(ShoppingListStoreReadModel source)
    {
        return new ShoppingListStoreContract(source.Id, source.Name);
    }
}