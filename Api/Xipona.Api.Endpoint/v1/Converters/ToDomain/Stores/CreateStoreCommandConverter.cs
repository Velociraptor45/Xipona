using Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;
using Xipona.Api.Contracts.Stores.Commands.CreateStore;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Services.Creations;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Stores;

public class CreateStoreCommandConverter : IToDomainConverter<CreateStoreContract, CreateStoreCommand>
{
    public CreateStoreCommand ToDomain(CreateStoreContract source)
    {
        var sections = source.Sections.Select(
                s => new SectionCreation(new SectionName(s.Name), s.SortingIndex, s.IsDefaultSection));

        return new CreateStoreCommand(new StoreCreation(new StoreName(source.Name), sections));
    }
}