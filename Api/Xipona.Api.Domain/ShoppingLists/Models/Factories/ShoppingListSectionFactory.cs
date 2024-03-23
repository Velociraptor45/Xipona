using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;

public class ShoppingListSectionFactory : IShoppingListSectionFactory
{
    public IShoppingListSection Create(SectionId sectionId, IEnumerable<ShoppingListItem> shoppingListItems)
    {
        return new ShoppingListSection(sectionId, shoppingListItems);
    }

    public IShoppingListSection CreateEmpty(ISection section)
    {
        return new ShoppingListSection(section.Id, Enumerable.Empty<ShoppingListItem>());
    }

    public IShoppingListSection CreateEmpty(SectionId sectionId)
    {
        return new ShoppingListSection(sectionId, Enumerable.Empty<ShoppingListItem>());
    }
}