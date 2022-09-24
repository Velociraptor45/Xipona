using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class Ingredient
{
    public Ingredient(Guid id, Guid itemCategoryId, int quantityType, float quantity, Guid? defaultItemId,
        Guid? defaultItemTypeId)
    {
        Id = id;
        ItemCategoryId = itemCategoryId;
        QuantityType = quantityType;
        Quantity = quantity;
        DefaultItemId = defaultItemId;
        DefaultItemTypeId = defaultItemTypeId;
    }

    public Guid Id { get; }
    public Guid ItemCategoryId { get; }
    public int QuantityType { get; }
    public float Quantity { get; }
    public Guid? DefaultItemId { get; }
    public Guid? DefaultItemTypeId { get; }
}