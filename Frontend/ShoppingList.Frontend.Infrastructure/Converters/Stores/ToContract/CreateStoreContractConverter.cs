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
                new CreateSectionContract
                {
                    Name = s.Name,
                    SortingIndex = s.SortingIndex,
                    IsDefaultSection = s.IsDefaultSection
                });

            return new CreateStoreContract
            {
                Name = request.Store.Name,
                Sections = sections
            };
        }
    }
}