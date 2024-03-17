using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;

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