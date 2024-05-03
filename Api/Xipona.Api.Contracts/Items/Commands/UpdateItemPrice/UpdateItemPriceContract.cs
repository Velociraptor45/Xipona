using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice
{
    /// <summary>
    /// Represents a command to update the price of an item.
    /// </summary>
    public class UpdateItemPriceContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemTypeId"></param>
        /// <param name="storeId"></param>
        /// <param name="price"></param>
        public UpdateItemPriceContract(Guid? itemTypeId, Guid storeId, float price)
        {
            ItemTypeId = itemTypeId;
            StoreId = storeId;
            Price = price;
        }

        /// <summary>
        /// The ID of the item type. If null, the price of the item is updated. If not null, the price of the item type is updated.
        /// </summary>
        public Guid? ItemTypeId { get; set; }

        /// <summary>
        /// The ID of the store for which the price is updated.
        /// </summary>
        public Guid StoreId { get; set; }

        /// <summary>
        /// The new price of the item or item type.
        /// </summary>
        public float Price { get; set; }
    }
}