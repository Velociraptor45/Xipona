using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;

public class ShoppingListFactory : IShoppingListFactory
{
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;

    public ShoppingListFactory(IShoppingListSectionFactory shoppingListSectionFactory)
    {
        _shoppingListSectionFactory = shoppingListSectionFactory;
    }

    public IShoppingList Create(ShoppingListId id, StoreId storeId, DateTimeOffset? completionDate,
        IEnumerable<IShoppingListSection> sections)
    {
        return new ShoppingList(id, storeId, completionDate, sections);
    }

    public IShoppingList CreateNew(IStore store)
    {
        var sections = store.Sections.Select(s => _shoppingListSectionFactory.CreateEmpty(s));

        return new ShoppingList(ShoppingListId.New, store.Id, null, sections);
    }
}