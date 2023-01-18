using ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record EditedIngredient(Guid Id, Guid ItemCategoryId, int QuantityTypeId, float Quantity, Guid? DefaultItemId,
    Guid? DefaultItemTypeId, ItemCategorySelector ItemCategorySelector);