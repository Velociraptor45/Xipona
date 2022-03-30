using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
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