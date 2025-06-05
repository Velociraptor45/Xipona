using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Models.Factories;

public interface IShoppingListFactory
{
    IShoppingList Create(ShoppingListId id, StoreId storeId, DateTimeOffset? completionDate,
        IEnumerable<IShoppingListSection> sections, DateTimeOffset createdAt, IEnumerable<ItemDiscount> discounts,
        IEnumerable<ListDiscount> listDiscounts);

    IShoppingList CreateNew(IStore store);
}