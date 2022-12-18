using ShoppingList.Frontend.Redux.Shared.Actions;

namespace ShoppingList.Frontend.Redux.Recipes.Actions;
public record EditRecipeAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
