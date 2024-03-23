using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

public class ItemAmountForOneServing
{
    public ItemAmountForOneServing(
        ItemId itemId,
        ItemTypeId? itemTypeId,
        string itemName,
        QuantityType quantityType,
        string quantityLabel,
        Quantity quantity,
        StoreId defaultStoreId,
        bool addToShoppingListByDefault,
        IEnumerable<ItemAmountForOneServingAvailability> availabilities)
    {
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        ItemName = itemName;
        QuantityType = quantityType;
        QuantityLabel = quantityLabel;
        Quantity = quantity;
        DefaultStoreId = defaultStoreId;
        AddToShoppingListByDefault = addToShoppingListByDefault;
        Availabilities = availabilities.ToList();
    }

    public ItemId ItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
    public string ItemName { get; }
    public QuantityType QuantityType { get; }
    public string QuantityLabel { get; }
    public Quantity Quantity { get; }
    public StoreId DefaultStoreId { get; }
    public bool AddToShoppingListByDefault { get; }
    public IReadOnlyCollection<ItemAmountForOneServingAvailability> Availabilities { get; }
}