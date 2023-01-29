namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record SearchItemByItemCategoryResult(Guid ItemId, Guid? ItemTypeId, string Name,
    IReadOnlyCollection<SearchItemByItemCategoryAvailability> Availabilities)
{
    public string SelectIdentifier
    {
        get => $"{ItemId}{ItemTypeId?.ToString() ?? string.Empty}";
        set { _ = value; }
    }
}