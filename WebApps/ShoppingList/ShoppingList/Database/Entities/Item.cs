using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class Item
    {
        public uint ItemId { get; set; }
        public string Name { get; set; }
        public uint QuantityTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? PricePerQuantity { get; set; }

        public virtual QuantityType QuantityType { get; set; }
    }
}
