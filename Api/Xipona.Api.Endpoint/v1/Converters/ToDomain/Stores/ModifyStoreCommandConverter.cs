﻿using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Stores;

public class ModifyStoreCommandConverter : IToDomainConverter<ModifyStoreContract, ModifyStoreCommand>
{
    public ModifyStoreCommand ToDomain(ModifyStoreContract source)
    {
        var sections =
            source.Sections.Select(s => new SectionModification(
                s.Id is null ? null : new SectionId(s.Id.Value),
                new SectionName(s.Name),
                s.SortingIndex,
                s.IsDefaultSection));

        return new ModifyStoreCommand(new StoreModification(new StoreId(source.Id), new StoreName(source.Name), sections));
    }
}