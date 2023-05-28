using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

public interface IAddItemToShoppingListService
{
    Task AddItemAsync(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId, QuantityInBasket quantity);

    Task AddItemAsync(IShoppingList shoppingList, IItem item, SectionId? sectionId, QuantityInBasket quantity);

    Task AddItemWithTypeAsync(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity);

    Task AddItemWithTypeAsync(IShoppingList shoppingList, IItem item, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity);

    Task AddAsync(ShoppingListId shoppingListId, OfflineTolerantItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity);

    Task AddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd);
}