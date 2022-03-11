using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ItemCategoryContractExtensions
    {
        public static ItemCategory ToModel(this ItemCategoryContract contract)
        {
            return new ItemCategory(contract.Id, contract.Name);
        }
    }
}