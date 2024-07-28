using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList
{
    /// <summary>
    /// Represents a contract to add an item type to a shopping list.
    /// </summary>
    public class AddItemWithTypeToShoppingListContract
    {
        /// <summary>
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="quantity"></param>
        public AddItemWithTypeToShoppingListContract(Guid? sectionId, float quantity)
        {
            SectionId = sectionId;
            Quantity = quantity;
        }

        /// <summary>
        /// The ID of the store's section to which the item type should be added.
        /// </summary>
        public Guid? SectionId { get; set; }

        /// <summary>
        /// The quantity of the item type to add.
        /// </summary>
        public float Quantity { get; set; }
    }
}