using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions;
public record EditRecipeAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
