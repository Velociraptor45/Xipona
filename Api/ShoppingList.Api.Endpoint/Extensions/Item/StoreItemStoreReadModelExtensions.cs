using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class StoreItemStoreReadModelExtensions
    {
        public static StoreItemStoreContract ToContract(this StoreItemStoreReadModel readModel)
        {
            return new StoreItemStoreContract(readModel.Id.Value, readModel.Name,
                readModel.Sections.Select(s => s.ToContract()));
        }
    }
}