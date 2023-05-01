using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public class ItemAmountForOneServing
{
    public ItemAmountForOneServing(
        ItemId itemId,
        ItemTypeId? itemTypeId,
        QuantityType quantityType,
        string quantityLabel,
        Quantity quantity,
        StoreId defaultStoreId,
        bool addToShoppingListByDefault,
        IEnumerable<ItemAmountForOneServingAvailability> availabilities)
    {
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        QuantityType = quantityType;
        QuantityLabel = quantityLabel;
        Quantity = quantity;
        DefaultStoreId = defaultStoreId;
        AddToShoppingListByDefault = addToShoppingListByDefault;
        Availabilities = availabilities.ToList();
    }

    public ItemId ItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
    public QuantityType QuantityType { get; }
    public string QuantityLabel { get; }
    public Quantity Quantity { get; }
    public StoreId DefaultStoreId { get; }
    public bool AddToShoppingListByDefault { get; }
    public IReadOnlyCollection<ItemAmountForOneServingAvailability> Availabilities { get; }
}