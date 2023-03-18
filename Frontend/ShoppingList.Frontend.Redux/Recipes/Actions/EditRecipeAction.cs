using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
public record EditRecipeAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
