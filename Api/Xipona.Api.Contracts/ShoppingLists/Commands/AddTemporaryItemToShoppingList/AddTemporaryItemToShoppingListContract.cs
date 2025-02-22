using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList
{
    /// <summary>
    /// Represents a request to add a temporary item to a shopping list.
    /// </summary>
    public class AddTemporaryItemToShoppingListContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="sectionId"></param>
        /// <param name="temporaryItemId"></param>
        public AddTemporaryItemToShoppingListContract(string itemName, int quantityType,
            float quantity, decimal price, Guid sectionId, Guid temporaryItemId)
        {
            ItemName = itemName;
            QuantityType = quantityType;
            Quantity = quantity;
            Price = price;
            SectionId = sectionId;
            TemporaryItemId = temporaryItemId;
        }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// The type of the item's quantity.
        /// </summary>
        public int QuantityType { get; set; }

        /// <summary>
        /// The quantity of the item.
        /// </summary>
        public float Quantity { get; set; }

        /// <summary>
        /// The price of the item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The ID of the store's section to which the item should be added.
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// The ID with which the temporary item can be identified by a client without having to
        /// wait for the api request to be successful (e.g. when offline).
        /// </summary>
        public Guid TemporaryItemId { get; set; }
    }
}