using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
public record EditedRecipe(Guid Id, string Name, int NumberOfServings, IReadOnlyCollection<EditedIngredient> Ingredients,
    SortedSet<EditedPreparationStep> PreparationSteps, IReadOnlyCollection<Guid> RecipeTagIds)
        : ISortable<EditedPreparationStep>
{
    public int MinSortingIndex => PreparationSteps.Any() ? PreparationSteps.Min(s => s.SortingIndex) : 0;

    public int MaxSortingIndex => PreparationSteps.Any() ? PreparationSteps.Max(s => s.SortingIndex) : 0;
}