using ProjectHermes.ShoppingList.Frontend.WebApp.Store.Shared.Actions;
using System;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.Actions;

public record EditManufacturerAction : ISearchResultTriggerAction
{
    public Guid Id { get; init; }
}