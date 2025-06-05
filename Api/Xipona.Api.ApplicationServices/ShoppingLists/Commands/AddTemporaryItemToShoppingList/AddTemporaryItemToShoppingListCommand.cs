using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;

public class AddTemporaryItemToShoppingListCommand : ICommand<TemporaryShoppingListItemReadModel>
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