using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;

public class AddItemToShoppingListCommand : ICommand<bool>
{
    public AddItemToShoppingListCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId,
        SectionId? sectionId, QuantityInBasket quantity)
    {
        ShoppingListId = shoppingListId;
        ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        SectionId = sectionId;
        Quantity = quantity;
    }

    public ShoppingListId ShoppingListId { get; }
    public OfflineTolerantItemId ItemId { get; }
    public SectionId? SectionId { get; }
    public QuantityInBasket Quantity { get; }
}