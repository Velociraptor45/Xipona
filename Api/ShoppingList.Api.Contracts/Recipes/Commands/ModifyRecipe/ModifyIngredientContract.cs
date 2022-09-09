using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe
{
    public class ModifyIngredientContract
    {
        public ModifyIngredientContract(Guid? id, Guid itemCategoryId, int quantityType, float quantity)
        {
            Id = id;
            ItemCategoryId = itemCategoryId;
            QuantityType = quantityType;
            Quantity = quantity;
        }

        public Guid? Id { get; set; }
        public Guid ItemCategoryId { get; set; }
        public int QuantityType { get; set; }
        public float Quantity { get; set; }
    }
}