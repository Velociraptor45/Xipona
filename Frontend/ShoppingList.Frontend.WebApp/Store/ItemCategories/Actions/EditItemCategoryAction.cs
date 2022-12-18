using ProjectHermes.ShoppingList.Frontend.WebApp.Store.Shared.Actions;
using System;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.ItemCategories.Actions;

public record EditItemCategoryAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
