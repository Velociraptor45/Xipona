using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
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