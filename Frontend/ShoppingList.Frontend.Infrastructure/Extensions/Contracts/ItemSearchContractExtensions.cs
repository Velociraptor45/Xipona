using ShoppingList.Api.Contracts.Queries;
using ShoppingList.Frontend.Models.Index.Search;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class ItemSearchContractExtensions
    {
        public static ItemSearchResult ToModel(this ItemSearchContract contract)
        {
            return new ItemSearchResult(contract.Id, contract.Name, contract.Price, "€", contract.ItemCategoryName,
                contract.ManufacturerName);
        }
    }
}