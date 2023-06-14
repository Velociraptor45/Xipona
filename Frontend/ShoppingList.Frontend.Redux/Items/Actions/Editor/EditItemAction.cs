using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
public record EditItemAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}