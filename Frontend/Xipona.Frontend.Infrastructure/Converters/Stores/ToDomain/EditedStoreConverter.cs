using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.Stores.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Stores.ToDomain;

public class EditedStoreConverter : IToDomainConverter<StoreContract, EditedStore>
{
    public EditedStore ToDomain(StoreContract source)
    {
        var sections = source.Sections
            .Select(s => new EditedSection(Guid.NewGuid(), s.Id, s.Name, s.IsDefaultSection, s.SortingIndex));

        return new EditedStore(
            source.Id,
            source.Name,
            new SortedSet<EditedSection>(sections, new SortingIndexComparer()));
    }
}