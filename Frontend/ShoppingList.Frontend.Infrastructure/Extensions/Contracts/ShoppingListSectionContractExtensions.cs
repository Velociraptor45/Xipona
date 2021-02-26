using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Frontend.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ShoppingListSectionContractExtensions
    {
        public static ShoppingListSection ToModel(this ShoppingListSectionContract contract)
        {
            return new ShoppingListSection(contract.Id, contract.Name, contract.SortingIndex, contract.IsDefaultSection,
                contract.Items.Select(i => i.ToModel()));
        }
    }
}