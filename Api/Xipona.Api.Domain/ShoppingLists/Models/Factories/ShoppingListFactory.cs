using Xipona.Api.Core.Services;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Models.Factories;

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
        IEnumerable<IShoppingListSection> sections, DateTimeOffset createdAt, IEnumerable<ItemDiscount> discounts,
        IEnumerable<ListDiscount> listDiscounts)
    {
        return new ShoppingList(id, storeId, completionDate, sections, createdAt, discounts, listDiscounts);
    }

    public IShoppingList CreateNew(IStore store)
    {
        var sections = store.Sections.Select(s => _shoppingListSectionFactory.CreateEmpty(s));

        return new ShoppingList(ShoppingListId.New, store.Id, null, sections, _dateTimeService.UtcNow, [], []);
    }
}