using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;
using ShoppingList.Api.Endpoint.Extensions.Item;
using ShoppingList.Api.Endpoint.Extensions.Store;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class ShoppingListReadModelExtensions
    {
        public static ShoppingListContract ToContract(this ShoppingListReadModel readModel)
        {
            return new ShoppingListContract(readModel.Id.Value, readModel.Store.ToContract(),
                readModel.Items.Select(item => item.ToContract()), readModel.CompletionDate);
        }
    }
}