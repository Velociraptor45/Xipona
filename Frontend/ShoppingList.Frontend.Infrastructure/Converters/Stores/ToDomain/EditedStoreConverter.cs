using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToDomain;

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