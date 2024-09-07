using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList
{
    /// <summary>
    /// Represents a request to add an item to a shopping list.
    /// </summary>
    public class AddItemToShoppingListContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="sectionId"></param>
        /// <param name="quantity"></param>
        public AddItemToShoppingListContract(Guid itemId, Guid? sectionId, float quantity)
        {
            ItemId = itemId;
            SectionId = sectionId;
            Quantity = quantity;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The ID of the store's section to which the item should be added.
        /// </summary>
        public Guid? SectionId { get; set; }

        /// <summary>
        /// The quantity of the item to add.
        /// </summary>
        public float Quantity { get; set; }
    }
}