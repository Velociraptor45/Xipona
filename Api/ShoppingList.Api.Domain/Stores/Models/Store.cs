using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Store : IStore
{
    private readonly Sections _sections;

    public Store(StoreId id, StoreName name, bool isDeleted, Sections sections)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        _sections = sections;
    }

    public StoreId Id { get; }
    public StoreName Name { get; private set; }
    public bool IsDeleted { get; }
    public IReadOnlyCollection<ISection> Sections => _sections.AsReadOnly();

    public ISection GetDefaultSection()
    {
        return _sections.GetDefaultSection();
    }

    public bool ContainsSection(SectionId sectionId)
    {
        return _sections.Contains(sectionId);
    }

    public void ChangeName(StoreName name)
    {
        Name = name;
    }

    public async Task UpdateSectionsAsync(IEnumerable<SectionUpdate> sectionUpdates,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService)
    {
        await _sections.UpdateManyAsync(sectionUpdates, itemModificationService, shoppingListModificationService);
    }
}