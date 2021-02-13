using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class ShoppingListSectionReadModelExtensions
    {
        public static ShoppingListSectionContract ToContract(this ShoppingListSectionReadModel readModel)
        {
            return new ShoppingListSectionContract(readModel.Id.Value, readModel.Name, readModel.SortingIndex,
                readModel.IsDefaultSection, readModel.ItemReadModels.Select(i => i.ToContract()));
        }
    }
}