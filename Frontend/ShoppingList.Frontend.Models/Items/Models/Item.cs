using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public class Item
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public QuantityType QuantityType { get; set; }
        public float? QuantityInPacket { get; set; }
        public QuantityTypeInPacket QuantityInPacketType { get; set; }
        public Guid? ItemCategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public List<ItemType> ItemTypes { get; set; }
        public IReadOnlyCollection<ItemAvailability> Availabilities { get; set; }
    }
}