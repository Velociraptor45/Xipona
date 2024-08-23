using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices
{
    /// <summary>
    /// Represents the price of an item type in a store.
    /// </summary>
    public class ItemTypePrice
    {
        /// <summary>
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="price"></param>
        public ItemTypePrice(Guid typeId, decimal price)
        {
            TypeId = typeId;
            Price = price;
        }

        /// <summary>
        /// The ID of the item type.
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// The price of the item type. The store is defined in the outer contract (<see cref="ItemTypePrices"/>)
        /// </summary>
        public decimal Price { get; set; }
    }
}