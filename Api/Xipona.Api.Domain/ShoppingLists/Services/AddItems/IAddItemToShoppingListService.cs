using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.AddItems;

public interface IAddItemToShoppingListService
{
    Task AddItemAsync(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId, QuantityInBasket quantity);

    Task<ShoppingListItem> AddItemAsync(IShoppingList shoppingList, IItem item, SectionId? sectionId,
        QuantityInBasket quantity);

    Task AddItemWithTypeAsync(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity);

    Task AddItemWithTypeAsync(IShoppingList shoppingList, IItem item, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity);

    Task AddAsync(ShoppingListId shoppingListId, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity);

    Task AddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd);
}