﻿using ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ShoppingList.Frontend.Redux.Manufacturers.Actions;

public record SearchManufacturersFinishedAction(IList<ManufacturerSearchResult> SearchResults);
