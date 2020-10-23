﻿using System.Collections.Generic;

namespace ShoppingList.Contracts.CreateItem
{
    public class CreateItemContract
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public int QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public int QuantityTypeInPacket { get; set; }
        public int ItemCategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public IEnumerable<ItemInStoreContract> ItemInStores { get; set; }
    }
}