namespace Xipona.Frontend.Redux.ShoppingList.States;

public class SearchItemForShoppingListResult
{
    public SearchItemForShoppingListResult(Guid itemId, Guid? itemTypeId, string name, decimal price,
        int defaultQuantity, string priceLabel, string itemCategoryName, string manufacturerName,
        Guid defaultSectionId, bool isFavorite)
    {
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        Name = name;
        Price = price;
        DefaultQuantity = defaultQuantity;
        PriceLabel = priceLabel;
        ItemCategoryName = itemCategoryName;
        ManufacturerName = manufacturerName;
        DefaultSectionId = defaultSectionId;
        IsFavorite = isFavorite;
    }

    public Guid ItemId { get; set; }
    public Guid? ItemTypeId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int DefaultQuantity { get; }
    public string PriceLabel { get; }
    public string ItemCategoryName { get; }
    public string ManufacturerName { get; }
    public Guid DefaultSectionId { get; }
    public bool IsFavorite { get; }

    public string SelectIdentifier
    {
        get => $"{ItemId}{ItemTypeId?.ToString() ?? string.Empty}";
        set { _ = value; }
    }
}