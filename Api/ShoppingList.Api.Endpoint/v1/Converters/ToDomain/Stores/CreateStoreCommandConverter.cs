using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreCreations;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Stores
{
    public class CreateStoreCommandConverter : IToDomainConverter<CreateStoreContract, CreateStoreCommand>
    {
        public CreateStoreCommand ToDomain(CreateStoreContract source)
        {
            var sections =
                source.Sections.Select(s => new SectionCreation(s.Name, s.SortingIndex, s.IsDefaultSection));

            return new CreateStoreCommand(new StoreCreation(source.Name, sections));
        }
    }
}