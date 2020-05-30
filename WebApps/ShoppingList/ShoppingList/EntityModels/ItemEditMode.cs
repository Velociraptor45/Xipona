namespace ShoppingList.EntityModels
{
    public enum ItemEditMode
    {
        /// <summary>
        /// Create new item
        /// </summary>
        Create,
        /// <summary>
        /// Update an existig item's data, e.g. when the store changes the price
        /// -> a new databse entry will be created to be able to calculate
        /// old shopping lists with the old price
        /// </summary>
        Update,
        /// <summary>
        /// Change an existing item' data, e.g. typo in the item name
        /// -> no new database entry will be created
        /// </summary>
        Change
    }
}
