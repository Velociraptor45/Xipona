namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record EditorValidationResult(string? Name, IReadOnlyDictionary<Guid, string> IngredientItemCategory)
{
    public EditorValidationResult() : this(null, new Dictionary<Guid, string>(0))
    {
    }

    public bool HasErrors => Name is not null || IngredientItemCategory.Any();
}