using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class StoreSectionReadModelExtensions
    {
        public static StoreSectionContract ToContract(this StoreSectionReadModel readModel)
        {
            return new StoreSectionContract(readModel.Id.Value, readModel.Name, readModel.SortingIndex,
                readModel.IsDefaultSection);
        }
    }
}