using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

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