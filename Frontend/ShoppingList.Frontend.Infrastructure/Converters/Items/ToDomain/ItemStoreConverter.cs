using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain;

public class ItemStoreConverter : IToDomainConverter<ActiveStoreContract, ItemStore>
{
    public ItemStore ToDomain(ActiveStoreContract source)
    {
        var sections = source.Sections
            .Select(s => new ItemStoreSection(s.Id, s.Name, s.IsDefaultSection))
            .ToList();

        return new ItemStore(source.Id, source.Name, sections);
    }
}