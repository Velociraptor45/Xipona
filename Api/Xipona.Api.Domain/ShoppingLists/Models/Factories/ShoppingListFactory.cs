using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;

public class ShoppingListFactory : IShoppingListFactory
{
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly IDateTimeService _dateTimeService;

    public ShoppingListFactory(IShoppingListSectionFactory shoppingListSectionFactory, IDateTimeService dateTimeService)
    {
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _dateTimeService = dateTimeService;
    }

    public IShoppingList Create(ShoppingListId id, StoreId storeId, DateTimeOffset? completionDate,
        IEnumerable<IShoppingListSection> sections, DateTimeOffset createdAt)
    {
        return new ShoppingList(id, storeId, completionDate, sections, createdAt);
    }

    public IShoppingList CreateNew(IStore store)
    {
        var sections = store.Sections.Select(s => _shoppingListSectionFactory.CreateEmpty(s));

        return new ShoppingList(ShoppingListId.New, store.Id, null, sections, _dateTimeService.UtcNow);
    }
}