﻿namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
public record PriceUpdate(ShoppingListItem? Item, float Price, bool UpdatePriceForAllTypes,
    bool IsOpen, bool IsSaving);