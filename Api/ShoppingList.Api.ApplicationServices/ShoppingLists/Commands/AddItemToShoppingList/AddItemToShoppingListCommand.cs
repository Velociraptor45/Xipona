using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;

public class AddItemToShoppingListCommand : ICommand<bool>
{
    public AddItemToShoppingListCommand(ShoppingListId shoppingListId, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        ShoppingListId = shoppingListId;
        ItemId = itemId;
        SectionId = sectionId;
        Quantity = quantity;
    }

    public ShoppingListId ShoppingListId { get; }
    public ItemId ItemId { get; }
    public SectionId? SectionId { get; }
    public QuantityInBasket Quantity { get; }
}