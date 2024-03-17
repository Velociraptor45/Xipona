using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;

public interface IShoppingListSectionFactory
{
    IShoppingListSection Create(SectionId sectionId, IEnumerable<ShoppingListItem> shoppingListItems);

    IShoppingListSection CreateEmpty(ISection section);

    IShoppingListSection CreateEmpty(SectionId sectionId);
}