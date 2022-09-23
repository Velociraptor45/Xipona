using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe
{
    public class CreateIngredientContract
    {
        public CreateIngredientContract(Guid itemCategoryId, int quantityType, float quantity, Guid? defaultItemId,
            Guid? defaultItemTypeId)
        {
            ItemCategoryId = itemCategoryId;
            QuantityType = quantityType;
            Quantity = quantity;
            DefaultItemId = defaultItemId;
            DefaultItemTypeId = defaultItemTypeId;
        }

        public Guid ItemCategoryId { get; set; }
        public int QuantityType { get; set; }
        public float Quantity { get; set; }
        public Guid? DefaultItemId { get; set; }
        public Guid? DefaultItemTypeId { get; set; }
    }
}