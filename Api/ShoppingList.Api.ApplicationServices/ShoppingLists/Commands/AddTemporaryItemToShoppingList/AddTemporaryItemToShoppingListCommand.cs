using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;

public class AddTemporaryItemToShoppingListCommand : ICommand<bool>
{
    public AddTemporaryItemToShoppingListCommand(ShoppingListId shoppingListId, ItemName itemName,
        QuantityType quantityType, QuantityInBasket quantity, Price price, SectionId sectionId, TemporaryItemId temporaryItemId)
    {
        ShoppingListId = shoppingListId;
        ItemName = itemName;
        QuantityType = quantityType;
        Quantity = quantity;
        Price = price;
        SectionId = sectionId;
        TemporaryItemId = temporaryItemId;
    }

    public ShoppingListId ShoppingListId { get; }
    public ItemName ItemName { get; }
    public QuantityType QuantityType { get; }
    public QuantityInBasket Quantity { get; }
    public Price Price { get; }
    public SectionId SectionId { get; }
    public TemporaryItemId TemporaryItemId { get; }
}