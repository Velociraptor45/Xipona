using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Stores.ToDomain;

public class EditedStoreConverter : IToDomainConverter<StoreContract, EditedStore>
{
    public EditedStore ToDomain(StoreContract source)
    {
        return new EditedStore(
            source.Id,
            source.Name,
            source.Sections
                .Select(s => new EditedSection(s.Id, s.Name, s.IsDefaultSection, s.SortingIndex))
                .ToList());
    }
}