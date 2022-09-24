﻿using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public class IngredientModification
{
    public IngredientModification(IngredientId? id, ItemCategoryId itemCategoryId, IngredientQuantityType quantityType,
        IngredientQuantity quantity, ItemId? defaultItemId, ItemTypeId? defaultItemTypeId)
    {
        Id = id;
        ItemCategoryId = itemCategoryId;
        QuantityType = quantityType;
        Quantity = quantity;
        DefaultItemId = defaultItemId;
        DefaultItemTypeId = defaultItemTypeId;
    }

    public IngredientId? Id { get; }
    public ItemCategoryId ItemCategoryId { get; }
    public IngredientQuantityType QuantityType { get; }
    public IngredientQuantity Quantity { get; }
    public ItemId? DefaultItemId { get; }
    public ItemTypeId? DefaultItemTypeId { get; }
}