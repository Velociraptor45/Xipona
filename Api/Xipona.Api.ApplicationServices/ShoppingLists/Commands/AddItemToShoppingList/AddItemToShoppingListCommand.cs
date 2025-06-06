using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;

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