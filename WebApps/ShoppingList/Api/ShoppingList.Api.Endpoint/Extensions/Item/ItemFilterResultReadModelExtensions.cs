using ShoppingList.Api.Contracts.Queries.ItemFilterResults;
using ShoppingList.Api.Domain.Queries.ItemFilterResults;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class ItemFilterResultReadModelExtensions
    {
        public static ItemFilterResultContract ToContract(this ItemFilterResultReadModel readModel)
        {
            return new ItemFilterResultContract(readModel.Id.Value, readModel.ItemName);
        }
    }
}