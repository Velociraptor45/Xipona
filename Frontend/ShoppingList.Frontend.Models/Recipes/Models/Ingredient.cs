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
    public Guid ItemCategoryId { get; set; }
    public int QuantityType { get; set; }
    public float Quantity { get; set; }
    public Guid? DefaultItemId { get; set; }
    public Guid? DefaultItemTypeId { get; set; }
    public static Ingredient New => new(Guid.Empty, Guid.Empty, 0, 1, null, null);
}