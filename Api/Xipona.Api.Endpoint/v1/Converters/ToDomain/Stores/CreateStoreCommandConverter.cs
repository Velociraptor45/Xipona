﻿using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Stores;

public class CreateStoreCommandConverter : IToDomainConverter<CreateStoreContract, CreateStoreCommand>
{
    public CreateStoreCommand ToDomain(CreateStoreContract source)
    {
        var sections = source.Sections.Select(
                s => new SectionCreation(new SectionName(s.Name), s.SortingIndex, s.IsDefaultSection));

        return new CreateStoreCommand(new StoreCreation(new StoreName(source.Name), sections));
    }
}