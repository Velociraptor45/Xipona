using System;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get
{
    /// <summary>
    /// Represents the contract for an ingredient.
    /// </summary>
    public class IngredientContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="itemCategoryId"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantity"></param>
        /// <param name="defaultItemId"></param>
        /// <param name="defaultItemTypeId"></param>
        /// <param name="defaultStoreId"></param>
        /// <param name="addToShoppingListByDefault"></param>
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

        /// <summary>
        /// The ID of the ingredient.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the ingredient.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The ID of the ingredient's item category.
        /// </summary>
        public Guid ItemCategoryId { get; }

        /// <summary>
        /// The type of the ingredient's quantity.
        /// </summary>
        public int QuantityType { get; }

        /// <summary>
        /// The quantity of the ingredient.
        /// </summary>
        public float Quantity { get; }

        /// <summary>
        /// The ID of the default item chosen when adding the recipe to shopping lists.
        /// Null if no default item should is set.
        /// </summary>
        public Guid? DefaultItemId { get; }

        /// <summary>
        /// The ID of the default item type. Must be a valid item type for the item.
        /// Null if no default item is set or default item does not have a types.
        /// </summary>
        public Guid? DefaultItemTypeId { get; }

        /// <summary>
        /// The ID of the default store chosen when adding the recipe to shopping lists.
        /// Null if no default item is set.
        /// </summary>
        public Guid? DefaultStoreId { get; }

        /// <summary>
        /// Whether the ingredient should be added to the shopping list by default.
        /// True if it should be added, false if it should not be added, null if no default item is set.
        /// </summary>
        public bool? AddToShoppingListByDefault { get; }
    }
}