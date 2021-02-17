using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using StoreReadModels = ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class StoreReadModelExtensions
    {
        public static StoreContract ToContract(this StoreReadModels.StoreReadModel readModel)
        {
            return new StoreContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }

        public static StoreContract ToContract(this StoreReadModel readModel)
        {
            return new StoreContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}