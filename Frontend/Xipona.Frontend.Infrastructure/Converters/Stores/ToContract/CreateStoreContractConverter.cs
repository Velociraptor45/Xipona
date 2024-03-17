using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Stores.ToContract
{
    public class CreateStoreContractConverter : IToContractConverter<EditedStore, CreateStoreContract>
    {
        public CreateStoreContract ToContract(EditedStore store)
        {
            var sections = store.Sections.Select(s =>
                new CreateSectionContract(
                    s.Name,
                    s.SortingIndex,
                    s.IsDefaultSection));

            return new CreateStoreContract(
                store.Name,
                sections);
        }
    }
}