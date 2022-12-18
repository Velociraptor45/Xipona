using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.Actions;

public record SearchManufacturersFinishedAction(IList<ManufacturerSearchResult> SearchResults);
