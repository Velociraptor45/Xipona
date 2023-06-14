using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions;
public record EditStoreAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}