using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

using CommonModels = ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Store
{
    public static class CreateStoreContractExtensions
    {
        public static CommonModels.Store ToDomain(this CreateStoreContract contract)
        {
            return new CommonModels.Store(new StoreId(0),
                contract.Name,
                false);
        }
    }
}