using Fluxor;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record RecipeState(
    IReadOnlyCollection<IngredientQuantityType> IngredientQuantityTypes,
    IReadOnlyCollection<RecipeTag> RecipeTags,
    RecipeSearch Search,
    RecipeEditor Editor);

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
                new List<RecipeSearchResult>(0)),
            new RecipeEditor(
                null,
                false));
    }
}