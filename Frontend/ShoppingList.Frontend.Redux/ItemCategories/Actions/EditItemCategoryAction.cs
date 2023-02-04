using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;

public record EditItemCategoryAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
