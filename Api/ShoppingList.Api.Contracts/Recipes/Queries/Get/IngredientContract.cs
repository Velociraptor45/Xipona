using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get
{
    public class IngredientContract
    {
        public IngredientContract(Guid id, string name, Guid itemCategoryId, int quantityType, float quantity,
            Guid? defaultItemId, Guid? defaultItemTypeId, Guid? defaultStoreId, bool? addToShoppingListByDefault)
        {
            Id = id;
            Name = name;
            ItemCategoryId = itemCategoryId;
            QuantityType = quantityType;
            Quantity = quantity;
            DefaultItemId = defaultItemId;
            DefaultItemTypeId = defaultItemTypeId;
            DefaultStoreId = defaultStoreId;
            AddToShoppingListByDefault = addToShoppingListByDefault;
        }

        public Guid Id { get; }
        public string Name { get; }
        public Guid ItemCategoryId { get; }
        public int QuantityType { get; }
        public float Quantity { get; }
        public Guid? DefaultItemId { get; }
        public Guid? DefaultItemTypeId { get; }
        public Guid? DefaultStoreId { get; }
        public bool? AddToShoppingListByDefault { get; }
    }
}