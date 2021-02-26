using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class ShoppingListReadModelExtensions
    {
        public static ShoppingListContract ToContract(this ShoppingListReadModel readModel)
        {
            return new ShoppingListContract(readModel.Id.Value, readModel.Store.ToContract(),
                readModel.Sections.Select(section => section.ToContract()), readModel.CompletionDate);
        }
    }
}