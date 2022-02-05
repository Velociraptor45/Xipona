using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;

public class AddItemWithTypeToShoppingListCommand : ICommand<bool>
{
    public AddItemWithTypeToShoppingListCommand(ShoppingListId shoppingListId, ItemId itemId,
        ItemTypeId itemTypeId, SectionId? sectionId, float quantity)
    {
        ShoppingListId = shoppingListId;
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        SectionId = sectionId;
        Quantity = quantity;
    }

    public ShoppingListId ShoppingListId { get; }
    public ItemId ItemId { get; }
    public ItemTypeId ItemTypeId { get; }
    public SectionId? SectionId { get; }
    public float Quantity { get; }
}