using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe
{
    public class CreateIngredientContract
    {
        public CreateIngredientContract(Guid itemCategoryId, int quantityType, float quantity)
        {
            ItemCategoryId = itemCategoryId;
            QuantityType = quantityType;
            Quantity = quantity;
        }

        public Guid ItemCategoryId { get; set; }
        public int QuantityType { get; set; }
        public float Quantity { get; set; }
    }
}