using ProjectHermes.ShoppingList.Frontend.WebApp.Store.Shared.Actions;
using System;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Items.Actions;
public record EditItemAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}