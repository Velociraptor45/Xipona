namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record SearchItemByItemCategoryResult(Guid ItemId, Guid? ItemTypeId, string Name, string ManufacturerName,
    IReadOnlyCollection<SearchItemByItemCategoryAvailability> Availabilities)
{
    public string SelectIdentifier
    {
        get => $"{ItemId}{ItemTypeId?.ToString() ?? string.Empty}";
        set { _ = value; }
    }
}