using ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Stores;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToContract
{
    public class CreateStoreContractConverter : IToContractConverter<CreateStoreRequest, CreateStoreContract>
    {
        public CreateStoreContract ToContract(CreateStoreRequest request)
        {
            var sections = request.Store.Sections.Select(s =>
                new CreateSectionContract(
                    s.Name,
                    s.SortingIndex,
                    s.IsDefaultSection));

            return new CreateStoreContract(
                request.Store.Name,
                sections);
        }
    }
}