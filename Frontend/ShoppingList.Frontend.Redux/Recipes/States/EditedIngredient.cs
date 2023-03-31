namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record EditedIngredient(
    Guid Key,
    Guid Id,
    Guid ItemCategoryId,
    int QuantityTypeId,
    float Quantity,
    Guid? DefaultItemId,
    Guid? DefaultItemTypeId,
    ItemCategorySelector ItemCategorySelector,
    ItemSelector ItemSelector)
{
    public string SelectedItemCategoryName => ItemCategorySelector
        .ItemCategories
        .FirstOrDefault(ic => ic.Id == ItemCategoryId)?
        .Name ?? string.Empty;

    public string GetSelectedQuantityLabel(IEnumerable<IngredientQuantityType> quantityTypes)
    {
        return quantityTypes
            .FirstOrDefault(qt => qt.Id == QuantityTypeId)?
            .QuantityLabel ?? string.Empty;
    }
}