using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public class StoreItemStoreReadModel
{
    public StoreItemStoreReadModel(StoreId id, StoreName name, IEnumerable<StoreItemSectionReadModel> sections)
    {
        Id = id;
        Name = name;
        Sections = sections.ToList().AsReadOnly();
    }

    public StoreItemStoreReadModel(IStore store) :
        this(store.Id, store.Name, store.Sections.Select(s => new StoreItemSectionReadModel(s)))
    {
    }

    public StoreId Id { get; }
    public StoreName Name { get; }
    public IReadOnlyCollection<StoreItemSectionReadModel> Sections { get; }
}