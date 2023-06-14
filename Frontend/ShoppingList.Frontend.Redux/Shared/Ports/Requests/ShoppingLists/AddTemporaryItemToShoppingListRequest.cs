namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;

public class AddTemporaryItemToShoppingListRequest : IApiRequest
{
    public AddTemporaryItemToShoppingListRequest(Guid requestId, Guid shoppingListId, string itemName, int quantityType,
        float quantity, float price, Guid sectionId, Guid temporaryId)
    {
        RequestId = requestId;
        ShoppingListId = shoppingListId;
        ItemName = itemName;
        QuantityType = quantityType;
        Quantity = quantity;
        Price = price;
        SectionId = sectionId;
        TemporaryId = temporaryId;
    }

    public Guid RequestId { get; }
    public Guid ShoppingListId { get; }
    public string ItemName { get; }
    public int QuantityType { get; }
    public float Quantity { get; }
    public float Price { get; }
    public Guid SectionId { get; }
    public Guid TemporaryId { get; }
}