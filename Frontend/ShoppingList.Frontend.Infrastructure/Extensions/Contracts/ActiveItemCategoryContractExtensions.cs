using ShoppingList.Api.Contracts.Queries.AllActiveItemCategories;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ActiveItemCategoryContractExtensions
    {
        public static ItemCategory ToModel(this ActiveItemCategoryContract contract)
        {
            return new ItemCategory(contract.Id, contract.Name);
        }
    }
}