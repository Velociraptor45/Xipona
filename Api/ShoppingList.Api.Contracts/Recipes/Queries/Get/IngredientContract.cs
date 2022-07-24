using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get
{
    public class IngredientContract
    {
        public IngredientContract(Guid id, Guid itemCategoryId, int quantityType, float quantity)
        {
            Id = id;
            ItemCategoryId = itemCategoryId;
            QuantityType = quantityType;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public Guid ItemCategoryId { get; }
        public int QuantityType { get; }
        public float Quantity { get; }
    }
}