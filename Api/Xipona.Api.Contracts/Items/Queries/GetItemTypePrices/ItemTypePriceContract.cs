using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices
{
    /// <summary>
    /// Represents the price of an item type in a store.
    /// </summary>
    public class ItemTypePriceContract
    {
        /// <summary>
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="price"></param>
        /// <param name="name"></param>
        public ItemTypePriceContract(Guid typeId, decimal price, string name)
        {
            TypeId = typeId;
            Price = price;
            Name = name;
        }

        /// <summary>
        /// The ID of the item type.
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// The price of the item type. The store is defined in the outer contract (<see cref="ItemTypePricesContract"/>)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The name of the item type. Does not include the item's name.
        /// </summary>
        public string Name { get; }
    }
}