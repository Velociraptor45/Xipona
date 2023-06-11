using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public interface IStore
{
    StoreId Id { get; }
    StoreName Name { get; }
    bool IsDeleted { get; }
    IReadOnlyCollection<ISection> Sections { get; }

    void ChangeName(StoreName name);

    bool ContainsSection(SectionId sectionId);

    ISection GetDefaultSection();

    Task ModifySectionsAsync(IEnumerable<SectionModification> sectionModifications,
        IItemModificationService itemModificationService,
        IShoppingListModificationService shoppingListModificationService);

    void Delete();
}