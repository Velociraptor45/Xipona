using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;

public class ShoppingListSectionFactory : IShoppingListSectionFactory
{
    public IShoppingListSection Create(SectionId sectionId, IEnumerable<IShoppingListItem> shoppingListItems)
    {
        return new ShoppingListSection(sectionId, shoppingListItems);
    }

    public IShoppingListSection CreateEmpty(IStoreSection storeSection)
    {
        return new ShoppingListSection(storeSection.Id, Enumerable.Empty<IShoppingListItem>());
    }

    public IShoppingListSection CreateEmpty(SectionId sectionId)
    {
        return new ShoppingListSection(sectionId, Enumerable.Empty<IShoppingListItem>());
    }
}