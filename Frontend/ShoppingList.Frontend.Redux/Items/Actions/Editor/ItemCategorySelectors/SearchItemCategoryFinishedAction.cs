﻿using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
public record SearchItemCategoryFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> SearchResults);