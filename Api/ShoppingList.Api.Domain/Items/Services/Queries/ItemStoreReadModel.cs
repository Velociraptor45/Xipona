using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public class ItemStoreReadModel
{
    public ItemStoreReadModel(StoreId id, StoreName name, IEnumerable<ItemSectionReadModel> sections)
    {
        Id = id;
        Name = name;
        Sections = sections.ToList().AsReadOnly();
    }

    public ItemStoreReadModel(IStore store) :
        this(store.Id, store.Name, store.Sections.Select(s => new ItemSectionReadModel(s)))
    {
    }

    public StoreId Id { get; }
    public StoreName Name { get; }
    public IReadOnlyCollection<ItemSectionReadModel> Sections { get; }
}