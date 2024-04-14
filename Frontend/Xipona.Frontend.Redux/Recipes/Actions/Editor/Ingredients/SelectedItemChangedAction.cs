namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;

public record SelectedItemChangedAction(Guid IngredientKey, Guid ItemId, Guid? ItemTypeId);