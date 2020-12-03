using ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ActiveItemCategoryContractExtensions
    {
        public static ItemCategory ToModel(this ActiveItemCategoryContract contract)
        {
            return new ItemCategory(contract.Id, contract.Name);
        }
    }
}