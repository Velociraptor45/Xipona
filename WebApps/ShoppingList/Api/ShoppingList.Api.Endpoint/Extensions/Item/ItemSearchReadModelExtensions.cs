using ShoppingList.Api.Contracts.Queries;
using ShoppingList.Api.Domain.Queries.ItemSearch;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class ItemSearchReadModelExtensions
    {
        public static ItemSearchContract ToContract(this ItemSearchReadModel readModel)
        {
            return new ItemSearchContract(readModel.Id.Value, readModel.Name, readModel.DefaultQuantity, readModel.Price,
                readModel.ItemCategory.Name, readModel.Manufacturer?.Name ?? "");
        }
    }
}