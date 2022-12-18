using ShoppingList.Frontend.Redux.Shared.Actions;

namespace ShoppingList.Frontend.Redux.Manufacturers.Actions;

public record EditManufacturerAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}