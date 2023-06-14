using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Actions;

public record EditManufacturerAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}