using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain;

public class ItemStoreConverter : IToDomainConverter<StoreForItemContract, ItemStore>
{
    public ItemStore ToDomain(StoreForItemContract source)
    {
        var sections = source.Sections
            .Select(s => new ItemStoreSection(s.Id, s.Name, s.IsDefaultSection, s.SortingIndex))
            .ToList();

        return new ItemStore(source.Id, source.Name, sections);
    }
}