﻿using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Actions;

public record SearchManufacturersFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);