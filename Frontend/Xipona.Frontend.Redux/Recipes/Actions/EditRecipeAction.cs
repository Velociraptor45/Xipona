using Xipona.Frontend.Redux.Shared.Actions;

namespace Xipona.Frontend.Redux.Recipes.Actions;
public record EditRecipeAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
