using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
public record EditedIngredient(
    Guid Key,
    Guid Id,
    string Name,
    Guid ItemCategoryId,
    int QuantityTypeId,
    float Quantity,
    Guid? DefaultItemId,
    Guid? DefaultItemTypeId,
    Guid? DefaultStoreId,
    bool? AddToShoppingListByDefault,
    ItemCategorySelector ItemCategorySelector,
    ItemSelector ItemSelector)
{
    public string SelectedItemCategoryName => ItemCategorySelector
        .ItemCategories
        .FirstOrDefault(ic => ic.Id == ItemCategoryId)?
        .Name ?? string.Empty;

    public IEnumerable<SearchItemByItemCategoryAvailability> AvailableStoresOfItem => ItemSelector
            .Items
            .FirstOrDefault(i => i.ItemId == DefaultItemId && i.ItemTypeId == DefaultItemTypeId)?
            .Availabilities
        ?? Enumerable.Empty<SearchItemByItemCategoryAvailability>();

    public string GetSelectedQuantityLabel(IEnumerable<IngredientQuantityType> quantityTypes)
    {
        return quantityTypes
            .FirstOrDefault(qt => qt.Id == QuantityTypeId)?
            .QuantityLabel ?? string.Empty;
    }

    public static EditedIngredient GetInitial(IEnumerable<IngredientQuantityType> quantityTypes)
    {
        return new EditedIngredient(
            Guid.NewGuid(),
            Guid.Empty,
            string.Empty,
            Guid.Empty,
            quantityTypes.First().Id,
            1,
            null,
            null,
            null,
            null,
            new ItemCategorySelector(
                new List<ItemCategorySearchResult>(0),
                string.Empty),
            new ItemSelector(new List<SearchItemByItemCategoryResult>(0)));
    }
}