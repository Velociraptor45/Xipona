using ShoppingList.Frontend.Redux.Shared.Actions;

namespace ShoppingList.Frontend.Redux.ItemCategories.Actions;

public record EditItemCategoryAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}
