using System;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.Actions;

public record UpdateSearchResultsAfterSaveAction(Guid Id, string Name);