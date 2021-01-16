using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class CreateStoreRequestExtensions
    {
        public static CreateStoreContract ToContract(this CreateStoreRequest request)
        {
            return new CreateStoreContract()
            {
                Name = request.Name
            };
        }
    }
}