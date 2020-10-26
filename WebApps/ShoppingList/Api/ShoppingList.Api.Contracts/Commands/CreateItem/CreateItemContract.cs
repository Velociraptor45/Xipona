using ShoppingList.Api.Contracts.Commands.SharedContracts;
using System.Collections.Generic;

namespace ShoppingList.Api.Contracts.Commands.CreateItem
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
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}