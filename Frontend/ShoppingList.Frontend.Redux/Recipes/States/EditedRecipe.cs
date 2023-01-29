using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record EditedRecipe(Guid Id, string Name, IReadOnlyCollection<EditedIngredient> Ingredients,
    SortedSet<EditedPreparationStep> PreparationSteps) : ISortable<EditedPreparationStep>
{
    public int MinSortingIndex => PreparationSteps.Any() ? PreparationSteps.Min(s => s.SortingIndex) : 0;

    public int MaxSortingIndex => PreparationSteps.Any() ? PreparationSteps.Max(s => s.SortingIndex) : 0;
}