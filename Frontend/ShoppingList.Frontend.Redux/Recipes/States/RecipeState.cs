using Fluxor;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record RecipeState(
    IReadOnlyCollection<IngredientQuantityType> IngredientQuantityTypes,
    IReadOnlyCollection<RecipeTag> RecipeTags,
    RecipeSearch Search,
    RecipeEditor Editor)
{
    public IEnumerable<string> GetTagNamesFor(IEnumerable<Guid> recipeTagIds)
    {
        var tagDict = RecipeTags.ToDictionary(x => x.Id, x => x.Name);

        foreach (var recipeTagId in recipeTagIds)
        {
            if (tagDict.TryGetValue(recipeTagId, out var tagName))
                yield return tagName;
        }
    }
}

public class RecipeFeatureState : Feature<RecipeState>
{
    public override string GetName()
    {
        return nameof(RecipeState);
    }

    protected override RecipeState GetInitialState()
    {
        return new RecipeState(
            new List<IngredientQuantityType>(0),
            new List<RecipeTag>(0),
            new RecipeSearch(
                false,
                new List<RecipeSearchResult>(0),
                new List<Guid>(0)),
            new RecipeEditor(
                null,
                string.Empty,
                false,
                false,
                null));
    }
}