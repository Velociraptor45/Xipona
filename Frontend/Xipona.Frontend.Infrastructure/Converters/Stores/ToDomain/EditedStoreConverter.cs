using Xipona.Api.Contracts.Stores.Queries.Get;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using Xipona.Frontend.Redux.Stores.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xipona.Frontend.Infrastructure.Converters.Stores.ToDomain;

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