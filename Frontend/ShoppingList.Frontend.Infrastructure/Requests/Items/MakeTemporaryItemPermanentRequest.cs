using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class MakeTemporaryItemPermanentRequest
    {
        private readonly IEnumerable<ItemAvailability> _availabilities;

        public MakeTemporaryItemPermanentRequest(Guid itemId, string name, string comment, int quantityType,
            float? quantityInPacket, int? quantityTypeInPacket, Guid itemCategoryId, Guid? manufacturerId,
            IEnumerable<ItemAvailability> availabilities)
        {
            ItemId = itemId;
            Name = name;
            Comment = comment;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            _availabilities = availabilities;
        }

        public Guid ItemId { get; }
        public string Name { get; }
        public string Comment { get; }
        public int QuantityType { get; }
        public float? QuantityInPacket { get; }
        public int? QuantityTypeInPacket { get; }
        public Guid ItemCategoryId { get; }
        public Guid? ManufacturerId { get; }

        public IReadOnlyCollection<ItemAvailability> Availabilities => _availabilities.ToList().AsReadOnly();
    }
}