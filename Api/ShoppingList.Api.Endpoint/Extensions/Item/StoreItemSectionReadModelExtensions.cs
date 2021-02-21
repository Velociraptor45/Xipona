using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class StoreItemSectionReadModelExtensions
    {
        public static StoreItemSectionContract ToContract(this StoreItemSectionReadModel readModel)
        {
            return new StoreItemSectionContract(readModel.Id.Value, readModel.Name, readModel.SortingIndex);
        }
    }
}