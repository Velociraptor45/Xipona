using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

public class StoreItemStoreReadModel
{
    public StoreItemStoreReadModel(StoreId id, string name, IEnumerable<StoreSectionReadModel> sections)
    {
        Id = id;
        Name = name;
        Sections = sections.ToList().AsReadOnly();
    }

    public StoreItemStoreReadModel(IStore store) :
        this(store.Id, store.Name, store.Sections.Select(s => new StoreSectionReadModel(s)))
    {
    }

    public StoreId Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<StoreSectionReadModel> Sections { get; }
}