using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

public class StoreReadModel
{
    public StoreReadModel(StoreId id, StoreName name, int itemCount,
        IEnumerable<SectionReadModel> sections)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ItemCount = itemCount;
        Sections = sections?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(sections));
    }

    public StoreReadModel(IStore store, int itemCount) :
        this(store.Id, store.Name, itemCount, store.Sections.Select(s => new SectionReadModel(s)))
    {
    }

    public StoreId Id { get; }
    public StoreName Name { get; }
    public int ItemCount { get; }
    public IReadOnlyCollection<SectionReadModel> Sections { get; }
}