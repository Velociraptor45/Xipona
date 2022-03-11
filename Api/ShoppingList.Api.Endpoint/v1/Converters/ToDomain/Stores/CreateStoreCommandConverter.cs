using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Stores
{
    public class CreateStoreCommandConverter : IToDomainConverter<CreateStoreContract, CreateStoreCommand>
    {
        public CreateStoreCommand ToDomain(CreateStoreContract source)
        {
            var sections =
                source.Sections.Select(s => new SectionCreationInfo(s.Name, s.SortingIndex, s.IsDefaultSection));

            return new CreateStoreCommand(new StoreCreationInfo(source.Name, sections));
        }
    }
}