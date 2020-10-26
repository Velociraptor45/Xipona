using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Converters
{
    public static class ShoppingListContractConverter
    {
        public static ShoppingListContract ToContract(this ShoppingListReadModel readModel)
        {
            return new ShoppingListContract(readModel.Id.Value, readModel.Store.ToContract(),
                readModel.Items.Select(item => item.ToContract()), readModel.CompletionDate);
        }
    }
}