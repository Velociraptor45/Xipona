using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class ItemFilterResultReadModelExtensions
    {
        public static ItemFilterResultContract ToContract(this ItemFilterResultReadModel readModel)
        {
            return new ItemFilterResultContract(readModel.Id.Value, readModel.ItemName);
        }
    }
}