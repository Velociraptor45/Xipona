using ShoppingList.Frontend.Redux.Shared.Actions;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor;
public record EditItemAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}