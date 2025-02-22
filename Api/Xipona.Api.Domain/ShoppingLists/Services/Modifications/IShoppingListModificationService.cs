using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;

public interface IShoppingListModificationService
{
    Task ChangeItemQuantityAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId, QuantityInBasket quantity);

    Task RemoveItemAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId);

    Task RemoveItemFromBasketAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId);

    Task PutItemInBasketAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId);

    Task FinishAsync(ShoppingListId shoppingListId, DateTimeOffset completionDate);

    Task RemoveSectionAsync(SectionId sectionId);

    Task RemoveItemAndItsTypesFromCurrentListAsync(ItemId itemId);

    Task<TemporaryShoppingListItemReadModel> AddTemporaryItemAsync(ShoppingListId shoppingListId, ItemName itemName,
        QuantityType quantityType,
        QuantityInBasket quantity, Price price, SectionId sectionId, TemporaryItemId temporaryItemId);

    Task AddDiscountAsync(ShoppingListId id, Discount discount);
    Task RemoveDiscountAsync(ShoppingListId id, ItemId itemId, ItemTypeId? itemTypeId);
}